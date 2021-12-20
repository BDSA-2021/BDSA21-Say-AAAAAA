using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SELearning.Infrastructure.Authorization;
using System.Linq;
using SELearning.Core.Credibility;
using SELearning.Core.Collections;

namespace SELearning.Infrastructure.Tests;

public class PermissionAuthorizationHandlerTests
{
    AuthorizationHandlerContext HandleAsync_WithUserScore(ClaimsPrincipal user, bool returnPermissionService)
    {
        var requirement = new PermissionRequirement(new Permission[] { Permission.CreateComment });

        var authContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { requirement }, user, null);

        var permissionService = new Mock<IPermissionService>();
        permissionService.Setup(m => m.IsAllowed(It.IsNotNull<IDynamicDictionaryRead>(), It.IsNotNull<IEnumerable<Permission>>())).ReturnsAsync(returnPermissionService);

        var authHandler = new PermissionAuthorizationHandler(permissionService.Object, Enumerable.Empty<IAuthorizationContextPipelineOperation>());
        authHandler.HandleAsync(authContext).Wait();

        return authContext;
    }

    [Fact]
    public void HandleAsync_PermissionServiceReturnsTrue_YieldsHasSucceeded()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson") }));
        var authContext = HandleAsync_WithUserScore(user, true);
        Assert.True(authContext.HasSucceeded);
    }

    [Fact]
    public void HandleAsync_PermissionServiceReturnsFalse_YieldsHasFailed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson") }));
        var authContext = HandleAsync_WithUserScore(user, false);
        Assert.True(authContext.HasFailed);
    }
}
