using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using SELearning.Core.Content;
using SELearning.Core.Permission;

namespace SELearning.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ContentController : ControllerBase
{
    private readonly ILogger<ContentController> _logger;
    private readonly IContentService _service;

    public ContentController(ILogger<ContentController> logger, IContentService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// <c>GetContent</c> returns the content with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the content.</param>
    /// <returns>A content with the given ID if it exists, otherwise response type 404: Not Found.</returns>
    [Authorize]
    [HttpGet("{ID}")]
    [ProducesResponseType(typeof(ContentDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ContentDto>> GetContent(int ID)
    {
        try
        {
            return Ok(await _service.GetContent(ID));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>GetAllContent</c> returns all contents.
    /// </summary>
    /// <returns>all contents if they can be found, otherwise response type 404: Not Found.</returns>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ContentDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IReadOnlyCollection<ContentDto>>> GetAllContent()
    {
        try
        {
            return Ok(await _service.GetContent());
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>GetContentsBySectionID</c> returns all contents in the section with the given section ID.
    /// </summary>
    /// <param name="sectionID">The ID of the section.</param>
    /// <returns>A collection of contents in the section if it exists, otherwise response type 404: Not Found.</returns>
    [Authorize]
    [HttpGet("{sectionID}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ContentDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IReadOnlyCollection<ContentDto>>> GetContentsBySectionID(int sectionID)
    {
        try
        {
            return Ok(await _service.GetContentInSection(sectionID));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>CreateContent</c> creates a content.
    /// </summary>
    /// <param name="content">The record of the content.</param>
    /// <returns>A response type 201: Created if the serice can create it, otherwise response type 404: Not Found.</returns>
    [AuthorizePermission(Permission.CreateContent)] // TODO: Create the possibility to have an 'or' evaluation of rules in the permission attribute and policy provider.
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CreateContent(ContentCreateDto content)
    {
        try
        {
            await _service.AddContent(content);
            return CreatedAtRoute(nameof(GetContent), content);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>UpdateContent</c> updates the content with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the content.</param>
    /// <param name="content">The record of the updated content.</param>
    /// <returns>A response type 204: No Content if the content exists, otherwise response type 404: Not Found.</returns>
    [AuthorizePermission(Permission.EditAnyContent)] // TODO: Create the possibility to have an 'or' evaluation of rules in the permission attribute and policy provider.
    [HttpPut("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateContent(int ID, ContentUpdateDto content)
    {
        try
        {
            await _service.UpdateContent(ID, content);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>GetContent</c> deletes the content with the given ID, and its associated comments.
    /// </summary>
    /// <param name="ID">The ID of the content.</param>
    /// <returns>A response type 204: No Content if the content exists, otherwise response type 404: Not Found.</returns>
    [AuthorizePermission(Permission.DeleteAnyContent)] // TODO: Create the possibility to have an 'or' evaluation of rules in the permission attribute and policy provider.
    [HttpDelete("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteContent(int ID)
    {
        try
        {
            await _service.DeleteContent(ID);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>UpvoteContent</c> increases the rating of the content with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the content.</param>
    /// <returns>A response type 204: No Content if the content exists, otherwise response type 404: Not Found.</returns>
    [Authorize]
    [HttpPut("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpvoteContent(int ID)
    {
        try
        {
            await _service.IncreaseContentRating(ID);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>DownvoteContent</c> decreases the rating of the content with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the content.</param>
    /// <returns>A response type 204: No Content if the content exists, otherwise response type 404: Not Found.</returns>
    [Authorize]
    [HttpPut("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DownvoteContent(int ID)
    {
        try
        {
            await _service.DecreaseContentRating(ID);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>GetSection</c> returns the section with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the section.</param>
    /// <returns>A section with the given ID if it exists, otherwise response type 404: Not Found.</returns>
    [Authorize]
    [HttpGet("{ID}")]
    [ProducesResponseType(typeof(SectionDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<SectionDto>> GetSection(int ID)
    {
        try
        {
            return Ok(await _service.GetSection(ID));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>GetSections</c> returns all sections.
    /// </summary>
    /// <returns>all sections if they can be found, otherwise response type 404: Not Found.</returns>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<SectionDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IReadOnlyCollection<SectionDto>>> GetAllSections()
    {
        try
        {
            return Ok(await _service.GetSections());
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>CreateSection</c> creates a section.
    /// </summary>
    /// <param name="section">The record of the section.</param>
    /// <returns>A response type 201: Created.</returns>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(201)]
    public async Task<IActionResult> CreateSection(SectionCreateDto section)
    {
        await _service.AddSection(section);
        return CreatedAtRoute(nameof(GetSection), section);
    }

    /// <summary>
    /// <c>UpdateSection</c> updates the section with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the section.</param>
    /// <param name="section">The record of the updated section.</param>
    /// <returns>A response type 204: No Content if the section exists, otherwise response type 404: Not Found.</returns>
    [Authorize]
    [HttpPut("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateSection(int ID, SectionUpdateDto section)
    {
        try
        {
            await _service.UpdateSection(ID, section);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>DeleteSection</c> deletes the section with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the section.</param>
    /// <returns>A response type 204: No Content if the section exists, otherwise response type 404: Not Found.</returns>
    [Authorize]
    [HttpDelete("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteSection(int ID)
    {
        try
        {
            await _service.DeleteSection(ID);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
}
