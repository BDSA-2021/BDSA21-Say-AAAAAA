using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SELearning.Infrastructure.Authorization;

namespace SELearning.Infrastructure.Tests;

public class PermissionAuthorizationHandlerTests
{
    [Fact]
    async public void HandleAsync_GivenPermittedUser_YieldsHasSucceeded()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson") }));
        var requirement = new PermissionRequirement(Permission.CreateContent);

        var authContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { requirement }, user, null);

        var permissionService = new Mock<IPermissionService>();
        permissionService.Setup(m => m.IsAllowed(user, Permission.CreateContent)).ReturnsAsync(true);

        var authHandler = new PermissionAuthorizationHandler(permissionService.Object);
        await authHandler.HandleAsync(authContext);

        Assert.True(authContext.HasSucceeded);
    }

    [Fact]
    async public void HandleAsync_GivenUnpermittedUser_YieldsHasFailed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson") }));
        var requirement = new PermissionRequirement(Permission.CreateContent);

        var authContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { requirement }, user, null);

        var permissionService = new Mock<IPermissionService>();
        permissionService.Setup(m => m.IsAllowed(user, Permission.CreateContent)).ReturnsAsync(false);

        var authHandler = new PermissionAuthorizationHandler(permissionService.Object);
        await authHandler.HandleAsync(authContext);

        Assert.True(authContext.HasFailed);
    }
}
