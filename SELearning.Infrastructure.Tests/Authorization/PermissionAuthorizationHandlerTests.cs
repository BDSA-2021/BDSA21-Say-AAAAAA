using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Collections;
using SELearning.Infrastructure.Authorization;
using SELearning.Infrastructure.Authorization.Handlers;
using SELearning.Infrastructure.Authorization.Pipeline;

namespace SELearning.Infrastructure.Tests.Authorization;

public class PermissionAuthorizationHandlerTests
{
    private static AuthorizationHandlerContext HandleAsync_WithUserScore(ClaimsPrincipal user,
        bool returnPermissionService)
    {
        var requirement = new PermissionRequirement(Permission.CreateComment);

        var authContext =
            new AuthorizationHandlerContext(new List<IAuthorizationRequirement> {requirement}, user, null);

        var permissionService = new Mock<IPermissionService>();
        permissionService
            .Setup(m => m.IsAllowed(It.IsNotNull<IDynamicDictionaryRead>(), It.IsNotNull<IEnumerable<Permission>>()))
            .ReturnsAsync(returnPermissionService);

        var authHandler = new PermissionAuthorizationHandler(permissionService.Object,
            Enumerable.Empty<IAuthorizationContextPipelineOperation>());
        authHandler.HandleAsync(authContext).Wait();

        return authContext;
    }

    [Fact]
    public void HandleAsync_PermissionServiceReturnsTrue_YieldsHasSucceeded()
    {
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(new List<Claim> {new(ClaimTypes.Name, "homer.simpson")}));
        var authContext = HandleAsync_WithUserScore(user, true);
        Assert.True(authContext.HasSucceeded);
    }

    [Fact]
    public void HandleAsync_PermissionServiceReturnsFalse_YieldsHasFailed()
    {
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(new List<Claim> {new(ClaimTypes.Name, "homer.simpson")}));
        var authContext = HandleAsync_WithUserScore(user, false);
        Assert.True(authContext.HasFailed);
    }
}
