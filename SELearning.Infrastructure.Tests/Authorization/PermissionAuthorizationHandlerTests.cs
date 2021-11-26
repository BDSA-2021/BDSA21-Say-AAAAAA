using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SELearning.Infrastructure.Authorization;

namespace SELearning.Infrastructure.Tests;

public class PermissionAuthorizationHandlerTests
{
    AuthorizationHandlerContext HandleAsync_WithIsPermitted(bool isPermitted)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson") }));
        var requirement = new PermissionRequirement(Permission.CreateContent);

        var authContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { requirement }, user, null);

        var permissionService = new Mock<IPermissionService>();
        permissionService.Setup(m => m.IsAllowed(user, Permission.CreateContent)).ReturnsAsync(isPermitted);

        var authHandler = new PermissionAuthorizationHandler(permissionService.Object);
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
