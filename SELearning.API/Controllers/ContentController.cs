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
    [HttpGet("{ID}")]
    [ProducesResponseType(typeof(ContentDTO), 200)] // OK
    [ProducesResponseType(404)] // Not Found
    public async Task<ActionResult<ContentDTO>> GetContent(string ID)
        => (await _repository.ReadAsync(ID)).ToActionResult();

    [Authorize]
    [HttpPost]
    [ProducesResponseType(201)] // Created
    public async Task<IActionResult> CreateContent(ContentDTO content)
    {
        var created = (await _repository.CreateAsync(content)).Item2;
        return CreatedAtRoute(nameof(GetContent), new { created.ID }, created);
    }

    [Authorize]
    [HttpPut("{ID}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> UpdateContent(int ID, ContentDTO content)
        => (await _repository.UpdateAsync(ID, content)).ToActionResult();

    [Authorize]
    [HttpDelete("{ID}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> DeleteContent(string ID)
        => (await _repository.DeleteContent(ID)).ToActionResult();
}
