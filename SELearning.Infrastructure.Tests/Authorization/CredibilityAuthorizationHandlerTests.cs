using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SELearning.Infrastructure.Authorization;

namespace SELearning.Infrastructure.Tests;

public class CredibilityAuthorizationHandlerTests
{
    AuthorizationHandlerContext HandleAsync_WithUserScore(int score)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson") }));
        var requirement = new CredibilityPermissionRequirement(1000);

        var authContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { requirement }, user, null);

        var permissionService = new Mock<ICredibilityService>();
        permissionService.Setup(m => m.GetCredibilityScore(user)).ReturnsAsync(score);

        var authHandler = new CredibilityAuthorizationHandler(permissionService.Object);
        authHandler.HandleAsync(authContext).Wait();

        return authContext;
    }

    [Fact]
    public void HandleAsync_GivenPermittedUser_YieldsHasSucceeded()
    {
        var authContext = HandleAsync_WithUserScore(1001);
        Assert.True(authContext.HasSucceeded);
    }

    [Fact]
    public void HandleAsync_GivenUnpermittedUser_YieldsHasFailed()
    {
        var authContext = HandleAsync_WithUserScore(999);
        Assert.True(authContext.HasFailed);
    }
}
