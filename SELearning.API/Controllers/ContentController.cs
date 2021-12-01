using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using SELearning.Core.Content;

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
    [HttpGet("{ID}")]
    [ProducesResponseType(typeof(ContentDTO), 200)] // OK
    [ProducesResponseType(404)] // Not Found
    public async Task<ActionResult<ContentDTO>> GetContent(string ID)
        => (await _repository.GetContent(ID)).ToActionResult();

    [Authorize]
    [HttpPost]
    [ProducesResponseType(201)] // Created
    public async Task<IActionResult> CreateContent(ContentDTO content)
    {
        var (result, created) = await _repository.AddContent(content);
        return CreatedAtRoute(nameof(GetContent), new { created.ID }, created);
    }

    [Authorize]
    [HttpPut("{ID}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> UpdateContent(string ID, ContentDTO content)
        => (await _repository.UpdateContent(ID, content)).ToActionResult();

    [Authorize]
    [HttpDelete("{ID}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> DeleteContent(string ID)
        => (await _repository.DeleteContent(ID)).ToActionResult();
}
