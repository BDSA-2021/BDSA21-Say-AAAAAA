using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using SELearning.Infrastructure.Authorization;

namespace SELearning.API.Controllers;

[Authorize]
[ApiController]
[Route("/Api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ContentController : ControllerBase
{
    private readonly ILogger<ContentController> _logger;
    private readonly IContentService _service;
    private readonly IResourceAuthorizationPermissionService _authService;
    private readonly IUserRepository _userRepository;

    public ContentController(
        ILogger<ContentController> logger,
        IContentService service,
        IUserRepository userRepository,
        IResourceAuthorizationPermissionService authService
    )
    {
        _logger = logger;
        _service = service;
        _userRepository = userRepository;
        _authService = authService;
    }

    /// <summary>
    /// <c>GetContent</c> returns the content with the given ID.
    /// </summary>
    /// <param name="id">The ID of the content.</param>
    /// <returns>A content with the given ID if it exists, otherwise response type 404: Not Found.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ContentDTO), 200)]
    [ProducesResponseType(404)]
    [ActionName(nameof(GetContent))]
    public async Task<ActionResult<ContentDTO>> GetContent(int id)
    {
        try
        {
            return Ok(await _service.GetContent(id));
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
    [ProducesResponseType(typeof(IReadOnlyCollection<ContentDTO>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IReadOnlyCollection<ContentDTO>>> GetAllContent()
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
    public async Task<IActionResult> CreateContent(ContentUserDTO content)
    {
        var user = await _userRepository.GetOrAddUser(new UserDTO(
            User.GetUserId()!,
            User.FindFirstValue(ClaimTypes.GivenName)
        ));

        var entity = new ContentCreateDto
        {
            Title = content.Title,
            Description = content.Description,
            VideoLink = content.VideoLink,
            SectionId = int.Parse(content.SectionId!),
            Author = user
        };

        var createdContent = await _service.AddContent(entity);
        return CreatedAtAction(nameof(GetContent), new { ID = createdContent.Id }, createdContent);
    }

    /// <summary>
    /// <c>UpdateContent</c> updates the content with the given ID.
    /// </summary>
    /// <param name="id">The ID of the content.</param>
    /// <param name="content">The record of the updated content.</param>
    /// <returns>A response type 204: No Content if the content exists, otherwise response type 404: Not Found.
    /// If the user is not authorized to update content, a response type 403: Forbidden is returned.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [AuthorizePermission(Permission.EditAnyContent, Permission.EditOwnContent)]
    public async Task<IActionResult> UpdateContent(int id, ContentUpdateDTO content)
    {
        try
        {
            var contentToBeUpdated = await _service.GetContent(id);

            var authResult = await _authService.Authorize(
                User,
                contentToBeUpdated,
                Permission.EditAnyContent, Permission.EditOwnContent);

            if (!authResult.Succeeded) return Forbid();

            await _service.UpdateContent(id, content);
            return NoContent();
        }
        catch (ContentNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>GetContent</c> deletes the content with the given ID, and its associated comments.
    /// </summary>
    /// <param name="id">The ID of the content.</param>
    /// <returns>A response type 204: No Content if the content exists, otherwise response type 404: Not Found.
    /// If the user is not authorized to delete content, a response type 403: Forbidden is returned.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [AuthorizePermission(Permission.DeleteAnyContent, Permission.DeleteOwnContent)]
    public async Task<IActionResult> DeleteContent(int id)
    {
        try
        {
            var contentToBeDeleted = await _service.GetContent(id);

            var authResult = await _authService.Authorize(
                User,
                contentToBeDeleted,
                Permission.DeleteAnyContent, Permission.DeleteOwnContent);

            if (!authResult.Succeeded) return Forbid();

            await _service.DeleteContent(id);
            return NoContent();
        }
        catch (ContentNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>UpvoteContent</c> increases the rating of the content with the given ID.
    /// </summary>
    /// <param name="id">The ID of the content.</param>
    /// <returns>A response type 204: No Content if the content exists, otherwise response type 404: Not Found.</returns>
    [HttpPut("{id:int}/Upvote")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [AuthorizePermission(Permission.Rate)]
    public async Task<IActionResult> UpvoteContent(int id)
    {
        try
        {
            await _service.IncreaseContentRating(id);
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
    /// <param name="id">The ID of the content.</param>
    /// <returns>A response type 204: No Content if the content exists, otherwise response type 404: Not Found.</returns>
    [HttpPut("{id:int}/Downvote")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [AuthorizePermission(Permission.Rate)]
    public async Task<IActionResult> DownvoteContent(int id)
    {
        try
        {
            await _service.DecreaseContentRating(id);
            return NoContent();
        }
        catch (ContentNotFoundException)
        {
            return NotFound();
        }
    }
}
