using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace SELearning.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ContentController : ControllerBase
{
    private readonly ILogger<ContentController> _logger;
    private readonly IContentRepository _repository;

    public ContentController(ILogger<ContentController> logger, IContentRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ContentDTO), 200)]
    [ProducesResponseType(404)]
    public Task<ActionResult<ContentDTO>> GetContent(int id)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public Task<ActionResult> CreateContent(ContentDTO content)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public Task<ActionResult> UpdateContent(int id, ContentDTO content)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public Task<ActionResult> DeleteContent(int id)
    {
        throw new NotImplementedException();
    }
}
