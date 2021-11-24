using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SELearning.API.Controllers;

// Required scope?

[Authorize]
[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly ICommentRepository _repository;

    public CommentController(ILogger<CommentController> logger, ICommentRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CommentDTO), 200)]
    [ProducesResponseType(404)]
    public Task<ActionResult<CommentDTO>> GetComment(int id)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public Task<ActionResult> CreateComment(CommentDTO comment)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public Task<ActionResult> UpdateComment(int id, CommentDTO comment)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public Task<ActionResult> DeleteComment(int id)
    {
        throw new NotImplementedException();
    }
}
