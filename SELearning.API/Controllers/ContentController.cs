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

    /// <summary>
    /// <c>GetContent</c> returns the content with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the content.</param>
    /// <returns>A content with the given ID if it exists, otherwise response type 404: Not Found.</returns>
    [HttpGet("{ID}")]
    [ProducesResponseType(typeof(ContentDTO), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ContentDTO>> GetContent(string ID)
        => (await _repository.GetContent(ID)).ToActionResult();

    /// <summary>
    /// <c>CreateContent</c> creates a content.
    /// </summary>
    /// <param name="content">The record of the content.</param>
    /// <returns>A response type 201: Created.</returns>
    [AuthorizePermission(Permission.CreateContent)] // TODO: Create the possibility to have an 'or' evaluation of rules in the permission attribute and policy provider.
    [HttpPost]
    [ProducesResponseType(201)]
    public async Task<IActionResult> CreateContent(ContentDTO content)
    {
        var (result, created) = await _repository.AddContent(content);
        return CreatedAtRoute(nameof(GetContent), new { created.ID }, created);
    }

    /// <summary>
    /// <c>UpdateContent</c> updates the content with the given ID.
    /// </summary>
    /// <param name="ID">The ID of the content.</param>
    /// <param name="content">The record of the updated content.</param>
    /// <returns></returns>
    [AuthorizePermission(Permission.EditAnyContent)] // TODO: Create the possibility to have an 'or' evaluation of rules in the permission attribute and policy provider.
    [HttpPut("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateContent(string ID, ContentDTO content)
        => (await _repository.UpdateContent(ID, content)).ToActionResult();

    /// <summary>
    /// <c>GetContent</c> deletes the content with the given ID, and its associated comments.
    /// </summary>
    /// <param name="ID">The ID of the content.</param>
    /// <returns>A response type 204: No Content if the content exists, otherwise response type 404: Not Found.</returns>
    [AuthorizePermission(Permission.DeleteAnyContent)] // TODO: Create the possibility to have an 'or' evaluation of rules in the permission attribute and policy provider.
    [HttpDelete("{ID}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteContent(string ID)
        => (await _repository.DeleteContent(ID)).ToActionResult();
}
