using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using SELearning.Core.Credibility;
using SELearning.Infrastructure.Authorization;

namespace SELearning.API.Controllers;

[ApiController]
[Authorize]
[Route("/Api/[controller]/me")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly ICredibilityService _credibilityService;

    public UserController(ILogger<UserController> logger, IUserRepository userRepo,
        ICredibilityService credibilityService)
    {
        _logger = logger;
        _userRepository = userRepo;
        _credibilityService = credibilityService;
    }

    [HttpGet]
    public async Task<ActionResult<UserDTO>> GetCurrentUser()
    {
        var user = await _userRepository.GetOrAddUser(
            new UserDTO(User.GetUserId()!, User.FindFirstValue(ClaimTypes.GivenName))
        );

        return Ok(user);
    }

    [HttpGet("credibility")]
    public async Task<ActionResult<UserCredibiityDTO>> GetCurrentUserCredibility()
    {
        var credibility = await _credibilityService.GetCredibilityScore(User);

        return Ok(new UserCredibiityDTO(credibility));
    }
}
