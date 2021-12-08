using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using SELearning.Core.Permission;

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

    /// <summary>
    /// <c>GetComment</c> returns the comment with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the comment.</param>
    /// <returns>A comment with the given ID if it exists, otherwise response type 404: Not Found.</returns>
    [HttpGet("{ID}")]
    [ProducesResponseType(typeof(CommentDTO), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<CommentDTO>> GetComment(int ID)
        => (await _repository.GetAsync(ID)).ToActionResult();

    /// <summary>
    /// <c>GetCommentsByContentID</c> returns all comments in the content with the given content ID.
    /// </summary>
    /// <param name="contentID">The ID of the content.</param>
    /// <returns>A collection of comments in the content if it exists, otherwise response type 404: Not Found.</returns>
    [HttpGet("{contentID}")]
    [ProducesResponseType(typeof(CommentDTO), 200)]
    [ProducesResponseType(404)]
    public async Task<IReadOnlyCollection<CommentDTO>> GetCommentsByContentID(int contentID)
        => await _repository.GetAsyncByContentID(contentID);

    /// <summary>
    /// <c>CreateComment</c> creates a comment in the content with the given content ID.
    /// </summary>
    /// <param name="contentID">The ID of the content.</param>
    /// <param name="comment">The record of the comment.</param>
    /// <returns>A response type 201: Created if the content exists, otherwise response type 404: Not Found.</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CreateComment(int contentID, CommentDTO comment)
    {
        var (result, created) = await _repository.CreateAsync(contentID, comment);
        if (result == OperationResult.NotFound) return result.ToActionResult();
        return CreatedAtRoute(nameof(GetComment), new { created.ID }, created);
    }

    /// <summary>
    /// <c>UpdateComment</c> updates the comment with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the comment.</param>
    /// <param name="comment">The record of the updated comment.</param>
    /// <returns>A response type 204: No Content if the comment exists, otherwise response type 404: Not Found.</returns>
    [HttpPut("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateComment(int ID, CommentDTO comment)
        => (await _repository.UpdateAsync(ID, comment)).Item1.ToActionResult();

    /// <summary>
    /// <c>DeleteComment</c> deletes the comment with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the comment.</param>
    /// <returns>A response type 204: No Content if the comment exists, otherwise response type 404: Not Found.</returns>
    [HttpDelete("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteComment(int ID)
        => (await _repository.DeleteAsync(ID)).ToActionResult();
}
