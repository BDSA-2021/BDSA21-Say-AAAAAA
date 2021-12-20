using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SELearning.Infrastructure.Authorization;
using System.Linq;
using SELearning.Core.Credibility;

namespace SELearning.Infrastructure.Tests;

public class CredibilityAuthorizationHandlerTests
{
    AuthorizationHandlerContext HandleAsync_WithUserScore(ClaimsPrincipal user, int score, IEnumerable<(Permission, int)> requiredScores)
    {
        var requirement = new CredibilityPermissionRequirement(requiredScores.ToArray());

        var authContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { requirement }, user, null);

        var permissionService = new Mock<ICredibilityService>();
        permissionService.Setup(m => m.GetCredibilityScore(user)).ReturnsAsync(score);

        var provider = new Mock<IProvider<ICredibilityService>>();
        provider.Setup(x => x.Get()).Returns(permissionService.Object);

        var authHandler = new PermissionAuthorizationHandler(provider.Object);
        authHandler.HandleAsync(authContext).Wait();

        return authContext;
    }

    [Fact]
    public void HandleAsync_GivenPermittedUser_YieldsHasSucceeded()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson") }));
        var authContext = HandleAsync_WithUserScore(user, 1001, new[] { (Permission.CreateContent, 1000) });
        Assert.True(authContext.HasSucceeded);
    }

    [Fact]
    public void HandleAsync_GivenUnpermittedUser_YieldsHasFailed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson") }));
        var authContext = HandleAsync_WithUserScore(user, 999, new[] { (Permission.EditOwnComment, 1000), (Permission.EditAnyComment, 1200) });
        Assert.True(authContext.HasFailed);
    }

    [Fact]
    public void HandleAsync_UserHasRoleModerator_YieldHasSucceeded()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson"), new Claim(ClaimTypes.Role, "Moderator"), new Claim(ClaimTypes.Role, "AnotherOne") }));
        var authContext = HandleAsync_WithUserScore(user, 0, new[] { (Permission.EditOwnComment, 450) });
        Assert.True(authContext.HasSucceeded);
    }

    [Fact]
    public void HandleAsync_GivenPermittedUserForOneOfMultipleRequirements_YieldsHasSucceeded()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson") }));
        var authContext = HandleAsync_WithUserScore(user, 1000, new[] { (Permission.EditOwnComment, 1200), (Permission.EditAnyComment, 1000) });
        Assert.True(authContext.HasSucceeded);
    }
}
