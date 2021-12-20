using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using SELearning.Infrastructure.Authorization;

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
    private readonly IResourceAuthorizationPermissionService _authService;

    public CommentController(
        ILogger<CommentController> logger,
        ICommentService service,
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
    /// <c>GetComment</c> returns the comment with the given ID.
    /// </summary>
    /// <param name="id">The ID of the comment.</param>
    /// <returns>A comment with the given ID if it exists, otherwise response type 404: Not Found.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Comment), 200)]
    [ProducesResponseType(404)]
    [ActionName(nameof(GetComment))]
    public async Task<ActionResult<Comment>> GetComment(int id)
    {
        try
        {
            return Ok(await _service.GetCommentFromCommentId(id));
        }
        catch (CommentNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>GetCommentsByContentID</c> returns all comments in the content with the given content ID.
    /// </summary>
    /// <param name="id">The ID of the content.</param>
    /// <returns>A collection of comments in the content if it exists, otherwise response type 404: Not Found.</returns>
    [HttpGet("content/{id:int}")]
    [ProducesResponseType(typeof(List<Comment>), 200)] // OK
    [ProducesResponseType(404)] // Not Found
    public async Task<ActionResult<List<Comment>>> GetCommentsByContentId(int id)
    {
        try
        {
            return Ok(await _service.GetCommentsFromContentId(id));
        }
        catch (ContentNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>CreateComment</c> creates a comment in the content with the given content ID.
    /// </summary>
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
    /// <param name="id">The ID of the comment.</param>
    /// <param name="comment">The record of the updated comment.</param>
    /// <returns>A response type 204: No Content if the comment exists, otherwise response type 404: Not Found.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    [AuthorizePermission(Permission.EditAnyComment, Permission.EditOwnComment)]
    public async Task<IActionResult> UpdateComment(int id, CommentUpdateDTO comment)
    {
        try
        {
            var commentToUpdate = await _service.GetCommentFromCommentId(id);
            var authResult = await _authService.Authorize(
                User,
                commentToUpdate,
                Permission.EditAnyComment, Permission.EditOwnComment);
            if (!authResult.Succeeded)
                return Forbid();

            await _service.UpdateComment(id, comment);
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
    /// <param name="id">The ID of the comment.</param>
    /// <returns>A response type 204: No Content if the comment exists, otherwise response type 404: Not Found.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [AuthorizePermission(Permission.DeleteAnyComment, Permission.DeleteOwnComment)]
    public async Task<IActionResult> DeleteComment(int id)
    {
        try
        {
            var commentToUpdate = await _service.GetCommentFromCommentId(id);
            var authResult = await _authService.Authorize(
                User,
                commentToUpdate,
                Permission.DeleteAnyComment, Permission.DeleteOwnComment
            );
            if (!authResult.Succeeded)
                return Forbid();

            await _service.RemoveComment(id);
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
    /// <param name="id">The ID of the comment.</param>
    /// <returns>A response type 204: No Content if the comment exists, otherwise response type 404: Not Found.</returns>
    [HttpPut("{id:int}/Upvote")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [AuthorizePermission(Permission.Rate)]
    public async Task<IActionResult> UpvoteComment(int id)
    {
        try
        {
            await _service.UpvoteComment(id);
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
    /// <param name="id">The ID of the comment.</param>
    /// <returns>A response type 204: No Content if the comment exists, otherwise response type 404: Not Found.</returns>
    [HttpPut("{id:int}/Downvote")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [AuthorizePermission(Permission.Rate)]
    public async Task<IActionResult> DownvoteComment(int id)
    {
        try
        {
            await _service.DownvoteComment(id);
            return NoContent();
        }
        catch (CommentNotFoundException)
        {
            return NotFound();
        }
    }
}
