using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using SELearning.Infrastructure.Authorization;

namespace SELearning.Infrastructure.Tests;

public class PermissionAuthorizationHandlerTests
{
    [Fact]
    async public void yadda()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson") }));
        var requirement = new OperationAuthorizationRequirement { Name = "Read" };

        var authzContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { requirement }, user, resource);

        var authzHandler = new PermissionAuthorizationHandler();
        await authzHandler.HandleAsync(authzContext);

        Assert.True(authzContext.HasSucceeded);
    }
}
