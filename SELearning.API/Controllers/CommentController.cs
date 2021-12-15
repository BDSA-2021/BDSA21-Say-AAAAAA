using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using SELearning.Infrastructure.Authorization;
using static SELearning.Infrastructure.Authorization.PermissionPolicyProvider;

namespace SELearning.API.Controllers;

[Authorize]
[ApiController]
[Route("/Api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class CommentController : ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly ICommentService _service;
    private readonly IUserRepository _userRepository;
    private readonly IAuthorizationService _authService;

    public CommentController(
        ILogger<CommentController> logger,
        ICommentService service,
        IUserRepository userRepository,
        IAuthorizationService authService
    )
    {
        _logger = logger;
        _service = service;
        _userRepository = userRepository;
        _authService = authService;
    }

    /// <summary>
    /// <c>GetComment</c> returns the comment with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the comment.</param>
    /// <returns>A comment with the given ID if it exists, otherwise response type 404: Not Found.</returns>
    [HttpGet("{ID}")]
    [ProducesResponseType(typeof(Comment), 200)]
    [ProducesResponseType(404)]
    [ActionName(nameof(GetComment))]
    public async Task<ActionResult<Comment>> GetComment(int ID)
    {
        try
        {
            return Ok(await _service.GetCommentFromCommentId(ID));
        }
        catch (CommentNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>GetCommentsByContentID</c> returns all comments in the content with the given content ID.
    /// </summary>
    /// <param name="contentID">The ID of the content.</param>
    /// <returns>A collection of comments in the content if it exists, otherwise response type 404: Not Found.</returns>
    [HttpGet("content/{contentID}")]
    [ProducesResponseType(typeof(List<Comment>), 200)] // OK
    [ProducesResponseType(404)] // Not Found
    public async Task<ActionResult<List<Comment>>> GetCommentsByContentID(int contentID)
    {
        try
        {
            return Ok(await _service.GetCommentsFromContentId(contentID));
        }
        catch (ContentNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>CreateComment</c> creates a comment in the content with the given content ID.
    /// </summary>
    /// <param name="contentID">The ID of the content.</param>
    /// <param name="comment">The record of the comment.</param>
    /// <returns>A response type 201: Created if the content exists, otherwise response type 404: Not Found.</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(404)]
    [AuthorizePermission(Permission.CreateComment)]
    public async Task<IActionResult> CreateComment(CommentUserDTO comment)
    {
        var user = await _userRepository.GetOrAddUser(new UserDTO(
            User.GetUserId()!,
            User.FindFirstValue(ClaimTypes.GivenName)
        ));

        try
        {
            var createdComment = await _service.PostComment(new CommentCreateDTO(
                user,
                comment.Text,
                comment.ContentId
            ));
            return CreatedAtAction(nameof(GetComment), new { ID = createdComment.Id }, createdComment);
        }
        catch (ContentNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>UpdateComment</c> updates the comment with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the comment.</param>
    /// <param name="comment">The record of the updated comment.</param>
    /// <returns>A response type 204: No Content if the comment exists, otherwise response type 404: Not Found.</returns>
    [HttpPut("{ID}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    [AuthorizePermission(Permission.EditAnyComment, Permission.EditOwnComment)]
    public async Task<IActionResult> UpdateComment(int ID, CommentUpdateDTO comment)
    {
        try
        {
            var commentToUpdate = await _service.GetCommentFromCommentId(ID);
            var authResult = await _authService.AuthorizeAsync(
                User,
                commentToUpdate,
                PermissionsToPolicyName(Permission.EditAnyComment, Permission.EditOwnComment));
            if (!authResult.Succeeded)
                return Forbid();

            await _service.UpdateComment(ID, comment);
            return NoContent();
        }
        catch (CommentNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>DeleteComment</c> deletes the comment with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the comment.</param>
    /// <returns>A response type 204: No Content if the comment exists, otherwise response type 404: Not Found.</returns>
    [HttpDelete("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [AuthorizePermission(Permission.DeleteAnyComment, Permission.DeleteOwnComment)]
    public async Task<IActionResult> DeleteComment(int ID)
    {
        try
        {
            var commentToUpdate = await _service.GetCommentFromCommentId(ID);
            AuthorizationResult authResult = await _authService.AuthorizeAsync(
                User,
                commentToUpdate,
                PermissionsToPolicyName(Permission.DeleteAnyComment, Permission.DeleteOwnComment));
            if (!authResult.Succeeded)
                return Forbid();

            await _service.RemoveComment(ID);
            return NoContent();
        }
        catch (CommentNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>UpvoteComment</c> increases the rating of the comment with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the comment.</param>
    /// <returns>A response type 204: No Content if the comment exists, otherwise response type 404: Not Found.</returns>
    [HttpPut("{ID}/Upvote")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [AuthorizePermission(Permission.Rate)]
    public async Task<IActionResult> UpvoteComment(int ID)
    {
        try
        {
            await _service.UpvoteComment(ID);
            return NoContent();
        }
        catch (CommentNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>DownvoteComment</c> decreases the rating of the comment with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the comment.</param>
    /// <returns>A response type 204: No Content if the comment exists, otherwise response type 404: Not Found.</returns>
    [HttpPut("{ID}/Downvote")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [AuthorizePermission(Permission.Rate)]
    public async Task<IActionResult> DownvoteComment(int ID)
    {
        try
        {
            await _service.DownvoteComment(ID);
            return NoContent();
        }
        catch (CommentNotFoundException)
        {
            return NotFound();
        }
    }
}
