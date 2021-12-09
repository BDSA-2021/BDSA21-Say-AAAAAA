using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SELearning.Infrastructure.Authorization;
using System.Linq;
using SELearning.Core.User;

namespace SELearning.Infrastructure.Tests;

record AuthoredResource(User Author) : IAuthored;

public class AuthoredCredibilityAuthorizationHandlerTests
{
    ClaimsPrincipal _user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "homer.simpson") }));

    User _userBart = new User { Id = "bart.simpson", Name = "Bart Simpson" };
    User _userHomer = new User { Id = "homer.simpson", Name = "Homer Simpson" };

    AuthorizationHandlerContext HandleAsync_WithPermissionsAndResource(
        ClaimsPrincipal user,
        int score,
        IEnumerable<(Permission, int)> requiredScores,
        IAuthored resource)
    {
        var requirement = new CredibilityPermissionRequirement(requiredScores.ToArray());

        var authContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { requirement }, user, resource);

        var permissionService = new Mock<ICredibilityService>();
        permissionService.Setup(m => m.GetCredibilityScore(user)).ReturnsAsync(score);

        var provider = new Mock<IProvider<ICredibilityService>>();
        provider.Setup(x => x.Get()).Returns(permissionService.Object);

        var authHandler = new AuthoredCredibilityAuthorizationHandler(provider.Object);
        authHandler.HandleAsync(authContext).Wait();

        return authContext;
    }

    [Fact]
    public void HandleAsync_GivenWrongAuthor_YieldsHasFailed()
    {
        var resource = new AuthoredResource(_userBart);
        var authContext = HandleAsync_WithPermissionsAndResource(_user, 1001, new[] { (Permission.EditOwnComment, 1000) }, resource);
        Assert.True(authContext.HasFailed);
    }

    [Fact]
    public void HandleAsync_GivenCorrectAuthor_YieldsHasSucceeded()
    {
        var resource = new AuthoredResource(_userHomer);
        var authContext = HandleAsync_WithPermissionsAndResource(_user, 1001, new[] { (Permission.EditOwnComment, 1000) }, resource);
        Assert.True(authContext.HasSucceeded);
    }

    [Fact]
    public void HandleAsync_GivenCorrectAuthorButInsufficientCredibility_YieldsHasFailed()
    {
        var resource = new AuthoredResource(_userHomer);
        var authContext = HandleAsync_WithPermissionsAndResource(_user, 999, new[] { (Permission.EditOwnComment, 1000) }, resource);
        Assert.True(authContext.HasFailed);
    }

    [Fact]
    public void HandleAsync_GivenWrongAuthorAndInsufficientCredibility_YieldsHasFailed()
    {
        var resource = new AuthoredResource(_userBart);
        var authContext = HandleAsync_WithPermissionsAndResource(_user, 999, new[] { (Permission.EditOwnComment, 1000) }, resource);
        Assert.True(authContext.HasFailed);
    }

    [Fact]
    public void HandleAsync_GivenWrongAuthorButHasEditAccess_YieldsHasSucceeded()
    {
        var resource = new AuthoredResource(_userBart);
        var authContext = HandleAsync_WithPermissionsAndResource(_user, 10000, new[] { (Permission.EditOwnComment, 1000), (Permission.EditAnyComment, 10000) }, resource);
        Assert.True(authContext.HasSucceeded);
    }

    [Fact]
    public void HandleAsync_GivenWrongAuthorAndNoEditAccess_YieldsHasFailed()
    {
        var resource = new AuthoredResource(_userBart);
        var authContext = HandleAsync_WithPermissionsAndResource(_user, 2000, new[] { (Permission.EditOwnComment, 1000), (Permission.EditAnyComment, 10000) }, resource);
        Assert.True(authContext.HasFailed);
    }
}
