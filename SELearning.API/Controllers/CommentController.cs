using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using SELearning.Core.Comment;
using SELearning.Core.Permission;

namespace SELearning.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class CommentController : ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly ICommentService _service;

    public CommentController(ILogger<CommentController> logger, ICommentService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// <c>GetComment</c> returns the comment with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the comment.</param>
    /// <returns>A comment with the given ID if it exists, otherwise response type 404: Not Found.</returns>
    [HttpGet("{ID}")]
    [ProducesResponseType(typeof(Comment), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Comment>> GetComment(int id)
    {
        try
        {
            return Ok(await _service.GetCommentFromCommentId(id));
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>GetCommentsByContentID</c> returns all comments in the content with the given content ID.
    /// </summary>
    /// <param name="contentID">The ID of the content.</param>
    /// <returns>A collection of comments in the content if it exists, otherwise response type 404: Not Found.</returns>
    [HttpGet("{contentID}")]
    [ProducesResponseType(typeof(List<Comment>), 200)] // OK
    [ProducesResponseType(404)] // Not Found
    public async Task<ActionResult<List<Comment>>> GetCommentsByContentID(int contentID)
    {
        try
        {
            return Ok(await _service.GetCommentsFromContentId(contentID));
        }
        catch (Exception)
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
    [AuthorizePermission(Permission.CreateComment)] // TODO: Create the possibility to have an 'or' evaluation of rules in the permission attribute and policy provider.
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CreateComment(CommentCreateDTO comment)
    {
        try
        {
            await _service.PostComment(comment);
            return CreatedAtRoute(nameof(GetComment), comment.ContentId);
        }
        catch (Exception)
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
    [AuthorizePermission(Permission.EditAnyComment)] // TODO: Create the possibility to have an 'or' evaluation of rules in the permission attribute and policy provider. EditOwnComment missing
    [HttpPut("{ID}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> UpdateComment(int ID, CommentUpdateDTO comment)
    {
        try
        {
            await _service.UpdateComment(ID, comment);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>DeleteComment</c> deletes the comment with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the comment.</param>
    /// <returns>A response type 204: No Content if the comment exists, otherwise response type 404: Not Found.</returns>
    [AuthorizePermission(Permission.DeleteAnyComment)] // TODO: Create the possibility to have an 'or' evaluation of rules in the permission attribute and policy provider.
    [HttpDelete("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteComment(int ID)
    {
        try
        {
            await _service.RemoveComment(ID);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>UpvoteComment</c> increases the rating of the comment with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the comment.</param>
    /// <returns>A response type 204: No Content if the comment exists, otherwise response type 404: Not Found.</returns>
    [Authorize]
    [HttpPut("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpvoteComment(int ID)
    {
        try
        {
            await _service.UpvoteComment(ID);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>DownvoteComment</c> decreases the rating of the comment with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the comment.</param>
    /// <returns>A response type 204: No Content if the comment exists, otherwise response type 404: Not Found.</returns>
    [Authorize]
    [HttpPut("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DownvoteComment(int ID)
    {
        try
        {
            await _service.DownvoteComment(ID);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
}
