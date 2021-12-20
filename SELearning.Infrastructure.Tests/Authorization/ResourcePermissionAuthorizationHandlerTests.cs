using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SELearning.Infrastructure.Authorization;
using System.Linq;
using SELearning.Core.User;
using SELearning.Core.Credibility;
using SELearning.Core.Collections;

namespace SELearning.Infrastructure.Tests;

record AuthoredResource(UserDTO Author) : IAuthored;

public class ResourcePermissionAuthorizationHandlerTests
{
    ClaimsPrincipal _user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "homer.simpson") }));

    UserDTO _userBart = new UserDTO("bart.simpson", "Bart Simpson");
    UserDTO _userHomer = new UserDTO("homer.simpson", "Homer Simpson");

    AuthorizationHandlerContext HandleAsync_WithPermissionsAndResource(
        ClaimsPrincipal user,
        bool returnPermissionService,
        IAuthored resource)
    {
        var requirement = new ResourcePermissionRequirement(Permission.CreateComment);

        var authContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { requirement }, user, resource);

        var permissionService = new Mock<IResourcePermissionService>();
        permissionService.Setup(m => m.IsAllowed(It.IsNotNull<IDynamicDictionaryRead>(), It.IsNotNull<IEnumerable<Permission>>(), It.IsNotNull<object>()))
                            .ReturnsAsync(returnPermissionService);

        var authHandler = new ResourcePermissionAuthorizationHandler(permissionService.Object, Enumerable.Empty<IAuthorizationContextPipelineOperation>());
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
