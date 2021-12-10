using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using SELearning.Core.User;
using SELearning.Infrastructure.Authorization;

namespace SELearning.API.Controllers;

[ApiController]
[Authorize]
[Route("/Api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly ICredibilityService _credibilityService;

    public UserController(ILogger<UserController> logger, IUserRepository userRepo, ICredibilityService credibilityService)
    {
        _logger = logger;
        _userRepository = userRepo;
        _credibilityService = credibilityService;
    }

    [HttpGet("me")]
    public async Task<ActionResult<User>> GetCurrentUser()
    {
        User user = await _userRepository.GetOrAddUser(new UserDTO(User.GetUserId()!, User.FindFirstValue(ClaimTypes.GivenName)));

        return Ok(user);
    }

    [HttpGet("me/credibility")]
    public async Task<ActionResult<int>> GetCurrentUserCredibility()
    {
        _logger.LogDebug("Getting current user credibility...");
        int credibility = await _credibilityService.GetCredibilityScore(User);
        _logger.LogDebug("Current user has a credbility of {credibility}", credibility);
        
        return Ok(new {CurrentCredibility = credibility });
    }
}