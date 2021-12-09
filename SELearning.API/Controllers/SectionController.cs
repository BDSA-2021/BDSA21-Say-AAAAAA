using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using SELearning.Core.Permission;

namespace SELearning.API.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class SectionController : ControllerBase
{
    private readonly ILogger<SectionController> _logger;
    private readonly IContentService _service;
    private readonly IAuthorizationService _authService;

    public SectionController(ILogger<SectionController> logger, IContentService service, IAuthorizationService authService)
    {
        _logger = logger;
        _service = service;
        _authService = authService;
    }

    /// <summary>
    /// <c>GetContentsBySectionID</c> returns all contents in the section with the given section ID.
    /// </summary>
    /// <param name="sectionID">The ID of the section.</param>
    /// <returns>A collection of contents in the section if it exists, otherwise response type 404: Not Found.</returns>
    [HttpGet("{ID}/Content")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ContentDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IReadOnlyCollection<ContentDto>>> GetContentsBySectionID(int ID)
    {
        try
        {
            return Ok(await _service.GetContentInSection(ID));
        }
        catch (SectionNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>GetSection</c> returns the section with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the section.</param>
    /// <returns>A section with the given ID if it exists, otherwise response type 404: Not Found.</returns>
    [HttpGet("{ID}")]
    [ProducesResponseType(typeof(SectionDto), 200)]
    [ProducesResponseType(404)]
    [ActionName(nameof(GetSection))]
    public async Task<ActionResult<SectionDto>> GetSection(int ID)
    {
        try
        {
            return Ok(await _service.GetSection(ID));
        }
        catch (SectionNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>GetSections</c> returns all sections.
    /// </summary>
    /// <returns>all sections if they can be found, otherwise response type 404: Not Found.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<SectionDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IReadOnlyCollection<SectionDto>>> GetAllSections()
    {
        return Ok(await _service.GetSections());
    }

    /// <summary>
    /// <c>CreateSection</c> creates a section.
    /// </summary>
    /// <param name="section">The record of the section.</param>
    /// <returns>A response type 201: Created</returns>
    [AuthorizePermission(Permission.CreateContent)]
    [HttpPost]
    [ProducesResponseType(201)]
    public async Task<IActionResult> CreateSection(SectionCreateDto section)
    {
        var createdSection = await _service.AddSection(section);
        return CreatedAtAction(nameof(GetSection), new { ID = createdSection.Id }, createdSection);
    }

    /// <summary>
    /// <c>UpdateSection</c> updates the section with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the section.</param>
    /// <param name="section">The record of the updated section.</param>
    /// <returns>A response type 204: No Content if the section exists, otherwise response type 404: Not Found.</returns>
    [HttpPut("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateSection(int ID, SectionUpdateDto section)
    {
        try
        {
            SectionDto sectionToBeUpdated = await _service.GetSection(ID);

            var authResult = await _authService.AuthorizeAsync(User, sectionToBeUpdated, "PermissionEditOwnContent_OR_PermissionEditAnyContent");

            if(authResult.Succeeded)
            {
                await _service.UpdateSection(ID, section);
                return NoContent();
            }
            else
                return Forbid($"User is not allowed to update comment with {ID}");
        }
        catch (SectionNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>DeleteSection</c> deletes the section with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the section.</param>
    /// <returns>A response type 204: No Content if the section exists, otherwise response type 404: Not Found. If the user is not allowed then a 403 forbidden will be returned</returns>
    [HttpDelete("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> DeleteSection(int ID)
    {
        try
        {
            var result = await _service.GetSection(ID);

            var authResult = await _authService.AuthorizeAsync(User, result, "PermissionDeleteOwnContent_OR_PermissionDeleteAnyContent");

            if(authResult.Succeeded) 
            {
                await _service.DeleteSection(ID);
                return NoContent();
            }
            else
                return Forbid($"User is not allowed to delete comment with {ID}");
        }
        catch (SectionNotFoundException)
        {
            return NotFound();
        }
    }
}
