using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Collections;
using SELearning.Core.User;
using SELearning.Infrastructure.Authorization;
using SELearning.Infrastructure.Authorization.Handlers;
using SELearning.Infrastructure.Authorization.Pipeline;

namespace SELearning.Infrastructure.Tests.Authorization;

internal record AuthoredResource(UserDTO Author) : IAuthored;

public class ResourcePermissionAuthorizationHandlerTests
{
    private readonly ClaimsPrincipal _user = new(new ClaimsIdentity(new List<Claim>
        {new(ClaimTypes.NameIdentifier, "homer.simpson")}));

    private readonly UserDTO _userBart = new("bart.simpson", "Bart Simpson");
    private readonly UserDTO _userHomer = new("homer.simpson", "Homer Simpson");

    private static AuthorizationHandlerContext HandleAsync_WithPermissionsAndResource(
        ClaimsPrincipal user,
        bool returnPermissionService,
        IAuthored resource)
    {
        var requirement = new ResourcePermissionRequirement(Permission.CreateComment);

        var authContext =
            new AuthorizationHandlerContext(new List<IAuthorizationRequirement> {requirement}, user, resource);

        var permissionService = new Mock<IResourcePermissionService>();
        permissionService.Setup(m => m.IsAllowed(It.IsNotNull<IDynamicDictionaryRead>(),
                It.IsNotNull<IEnumerable<Permission>>(), It.IsNotNull<object>()))
            .ReturnsAsync(returnPermissionService);

        var authHandler = new ResourcePermissionAuthorizationHandler(permissionService.Object,
            Enumerable.Empty<IAuthorizationContextPipelineOperation>());
        authHandler.HandleAsync(authContext).Wait();

        return authContext;
    }

    [Fact]
    public void HandleAsync_PermissionServiceReturnFalse_YieldsHasFailed()
    {
        var resource = new AuthoredResource(_userBart);
        var authContext = HandleAsync_WithPermissionsAndResource(_user, false, resource);
        Assert.True(authContext.HasFailed);
    }

    [Fact]
    public void HandleAsync_PermissionServiceReturnTrue_YieldsHasSucceeded()
    {
        var resource = new AuthoredResource(_userHomer);
        var authContext = HandleAsync_WithPermissionsAndResource(_user, true, resource);
        Assert.True(authContext.HasSucceeded);
    }
}
