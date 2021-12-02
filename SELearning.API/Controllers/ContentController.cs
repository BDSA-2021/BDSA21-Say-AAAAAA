using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using SELearning.Core.Content;
using SELearning.Core.Permission;

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

    [HttpGet("{ID}")]
    [ProducesResponseType(typeof(ContentDTO), 200)] // OK
    [ProducesResponseType(404)] // Not Found
    public async Task<ActionResult<ContentDTO>> GetContent(string ID)
        => (await _repository.GetContent(ID)).ToActionResult();

    [AuthorizePermission(Permission.CreateContent)]
    [HttpPost]
    [ProducesResponseType(201)] // Created
    public async Task<IActionResult> CreateContent(ContentDTO content)
    {
        var (result, created) = await _repository.AddContent(content);
        return CreatedAtRoute(nameof(GetContent), new { created.ID }, created);
    }

    [AuthorizePermission(Permission.EditAnyContent)]
    [HttpPut("{ID}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> UpdateContent(string ID, ContentDTO content)
        => (await _repository.UpdateContent(ID, content)).ToActionResult();

    [AuthorizePermission(Permission.DeleteAnyContent)] // TODO: Create the possibility to have an 'or' evaluation of rules in the permission attribute and policy provider.
    [HttpDelete("{ID}")]
    [ProducesResponseType(204)] // No Content
    [ProducesResponseType(404)] // Not Found
    public async Task<IActionResult> DeleteContent(string ID)
        => (await _repository.DeleteContent(ID)).ToActionResult();
}
