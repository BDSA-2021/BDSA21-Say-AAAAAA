using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
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
    private readonly IAuthorizationService _authService;

    public ContentController(ILogger<ContentController> logger, IContentService service, IAuthorizationService authService)
    {
        _logger = logger;
        _service = service;
        _authService = authService;
    }

    /// <summary>
    /// <c>GetContent</c> returns the content with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the content.</param>
    /// <returns>A content with the given ID if it exists, otherwise response type 404: Not Found.</returns>
    [HttpGet("{ID}")]
    [ProducesResponseType(typeof(ContentDto), 200)]
    [ProducesResponseType(404)]
    [ActionName(nameof(GetContent))]
    public async Task<ActionResult<ContentDto>> GetContent(int ID)
    {
        try
        {
            return Ok(await _service.GetContent(ID));
        }
        catch (ContentNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>GetAllContent</c> returns all contents.
    /// </summary>
    /// <returns>all contents if they can be found, otherwise response type 404: Not Found.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ContentDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IReadOnlyCollection<ContentDto>>> GetAllContent()
    {
        return Ok(await _service.GetContent());
    }

    /// <summary>
    /// <c>CreateContent</c> creates a content.
    /// </summary>
    /// <param name="content">The record of the content.</param>
    /// <returns>A response type 201: Created</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [AuthorizePermission(Permission.CreateContent)]
    public async Task<IActionResult> CreateContent(ContentCreateDto content)
    {
        var createdContent = await _service.AddContent(content);
        return CreatedAtAction(nameof(GetContent), new { ID = createdContent.Id }, createdContent);
    }

    /// <summary>
    /// <c>UpdateContent</c> updates the content with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the content.</param>
    /// <param name="content">The record of the updated content.</param>
    /// <returns>A response type 204: No Content if the content exists, otherwise response type 404: Not Found.</returns>
    [HttpPut("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateContent(int ID, ContentUpdateDto content)
    {
        try
        {
            ContentDto contentToBeUpdated = await _service.GetContent(ID);

            var authResult = await _authService.AuthorizeAsync(User, contentToBeUpdated, "PermissionDeleteOwnContent OR PermissionDeleteAnyContent");

            if (authResult.Succeeded)
            {
                await _service.UpdateContent(ID, content);
                return NoContent();
            }
            else
                return Forbid($"User is not allowed to update content with id {ID}");
        }
        catch (ContentNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>GetContent</c> deletes the content with the given ID, and its associated comments.
    /// </summary>
    /// <param name="ID">The ID of the content.</param>
    /// <returns>A response type 204: No Content if the content exists, otherwise response type 404: Not Found.</returns>
    [HttpDelete("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteContent(int ID)
    {
        try
        {
            ContentDto contentToBeDeleted = await _service.GetContent(ID);

            var authResult = await _authService.AuthorizeAsync(User, contentToBeDeleted, "PermissionDeleteOwnContent OR PermissionDeleteAnyContent");

            if (authResult.Succeeded)
            {
                await _service.DeleteContent(ID);
                return NoContent();
            }
            else
                return Forbid($"User is not allowed to delete content with id {ID}");
        }
        catch (ContentNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>UpvoteContent</c> increases the rating of the content with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the content.</param>
    /// <returns>A response type 204: No Content if the content exists, otherwise response type 404: Not Found.</returns>
    [HttpPut("{ID}/Upvote")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [AuthorizePermission(Permission.Rate)]
    public async Task<IActionResult> UpvoteContent(int ID)
    {
        try
        {
            await _service.IncreaseContentRating(ID);
            return NoContent();
        }
        catch (ContentNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>DownvoteContent</c> decreases the rating of the content with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the content.</param>
    /// <returns>A response type 204: No Content if the content exists, otherwise response type 404: Not Found.</returns>
    [HttpPut("{ID}/Downvote")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [AuthorizePermission(Permission.Rate)]
    public async Task<IActionResult> DownvoteContent(int ID)
    {
        try
        {
            await _service.DecreaseContentRating(ID);
            return NoContent();
        }
        catch (ContentNotFoundException)
        {
            return NotFound();
        }
    }
}
