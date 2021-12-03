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
    private readonly ICommentRepository _repository;

    public CommentController(ILogger<CommentController> logger, ICommentRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [Authorize]
    [HttpGet("{ID}")]
    [ProducesResponseType(typeof(Comment), 200)] // OK
    [ProducesResponseType(404)] // Not Found
    public async Task<ActionResult<Comment>> GetComment(int id)
        => (await _repository.GetCommentByCommentId(id)).ToActionResult();

    [Authorize]
    [HttpGet("{contentID}")]
    [ProducesResponseType(typeof(List<Comment>), 200)] // OK
    [ProducesResponseType(404)] // Not Found
    public async Task<ActionResult<List<Comment>>> GetCommentsByContentID(int contentID)
    {
        var (created, result) = await _repository.GetCommentsByContentId(contentID);
        if (result == OperationResult.NotFound) return new NotFoundResult();
        return Ok(created);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(201)] // Created
    public async Task<IActionResult> CreateComment(int contentID, CommentCreateDTO comment)
    {
        var (result, created) = await _repository.AddComment(comment);
        if (result == OperationResult.NotFound) { return new NotFoundResult(); }
        return CreatedAtRoute(nameof(GetComment), new { created.Id }, created);
    }

    [Authorize]
    [HttpPut("{ID}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> UpdateComment(int ID, CommentUpdateDTO comment)
        => (await _repository.UpdateComment(ID, comment)).Item1.ToActionResult();

    [Authorize]
    [HttpDelete("{ID}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> DeleteComment(int ID)
        => (await _repository.RemoveComment(ID)).ToActionResult();
}
