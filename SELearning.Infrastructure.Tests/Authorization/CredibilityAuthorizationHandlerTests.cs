using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SELearning.Infrastructure.Authorization;

namespace SELearning.Infrastructure.Tests;

public class CredibilityAuthorizationHandlerTests
{
    AuthorizationHandlerContext HandleAsync_WithIsPermitted(bool isPermitted)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson") }));
        var requirement = new PermissionRequirement(Permission.CreateContent);

        var authContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { requirement }, user, null);

        var permissionService = new Mock<ICredibilityService>();
        permissionService.Setup(m => m.GetCredibilityScore(user)).ReturnsAsync(1000);

        var authHandler = new CredibilityAuthorizationHandler(permissionService.Object);
        authHandler.HandleAsync(authContext).Wait();

        return authContext;
    }

    [Fact]
    public void HandleAsync_GivenPermittedUser_YieldsHasSucceeded()
    {
        var authContext = HandleAsync_WithIsPermitted(true);
        Assert.True(authContext.HasSucceeded);
    }

    [Fact]
    public void HandleAsync_GivenUnpermittedUser_YieldsHasFailed()
    {
        var authContext = HandleAsync_WithIsPermitted(false);
        Assert.True(authContext.HasFailed);
    }
}
