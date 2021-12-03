using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using SELearning.Core.Comment;

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

    [Authorize]
    [HttpGet("{ID}")]
    [ProducesResponseType(typeof(Comment), 200)] // OK
    [ProducesResponseType(404)] // Not Found
    public async Task<ActionResult<Comment>> GetComment(int id)
    {
        try
        {
            return Ok(await _service.GetCommentFromCommentId(id));
        }
        catch (Exception)
        {
            return new NotFoundResult();
        }
    }

    [Authorize]
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
            return new NotFoundResult();
        }
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(201)] // Created
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> CreateComment(CommentCreateDTO comment)
    {
        try
        {
            await _service.PostComment(comment);
            return Created(nameof(GetComment), new { comment.ContentId });
        }
        catch (Exception)
        {
            return new NotFoundResult();
        }
    }

    [Authorize]
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
            return new NotFoundResult();
        }
    }

    [Authorize]
    [HttpDelete("{ID}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> DeleteComment(int ID)
    {
        try
        {
            await _service.RemoveComment(ID);
            return NoContent();
        }
        catch (Exception)
        {
            return new NotFoundResult();
        }
    }

    [Authorize]
    [HttpDelete("{ID}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> UpvoteComment(int ID)
    {
        try
        {
            await _service.UpvoteComment(ID);
            return NoContent();
        }
        catch (Exception)
        {
            return new NotFoundResult();
        }
    }

    [Authorize]
    [HttpDelete("{ID}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> DownvoteComment(int ID)
    {
        try
        {
            await _service.DownvoteComment(ID);
            return NoContent();
        }
        catch (Exception)
        {
            return new NotFoundResult();
        }
    }
}
