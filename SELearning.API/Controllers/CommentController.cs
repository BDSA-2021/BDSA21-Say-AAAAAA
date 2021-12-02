using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

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
    [ProducesResponseType(typeof(CommentDTO), 200)] // OK
    [ProducesResponseType(404)] // Not Found
    public async Task<ActionResult<CommentDTO>> GetComment(int id)
        => (await _repository.GetAsync(id)).ToActionResult();

    [Authorize]
    [HttpGet("{contentID}")]
    [ProducesResponseType(typeof(CommentDTO), 200)] // OK
    [ProducesResponseType(404)] // Not Found
    public async Task<IReadOnlyCollection<CommentDTO>> GetCommentsByContentID(int contentID)
        => await _repository.GetAsyncByContentID(contentID);

    [Authorize]
    [HttpPost]
    [ProducesResponseType(201)] // No Content
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> CreateComment(int contentID, CommentDTO comment)
    {
        var (result, created) = await _repository.CreateAsync(contentID, comment);
        if (result == OperationResult.NotFound) return result.ToActionResult();
        return CreatedAtRoute(nameof(GetComment), new { created.ID }, created);
    }

    [Authorize]
    [HttpPut("{ID}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> UpdateComment(int ID, CommentDTO comment)
        => (await _repository.UpdateAsync(ID, comment)).Item1.ToActionResult();

    [Authorize]
    [HttpDelete("{ID}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> DeleteComment(int ID)
        => (await _repository.DeleteAsync(ID)).ToActionResult();
}
