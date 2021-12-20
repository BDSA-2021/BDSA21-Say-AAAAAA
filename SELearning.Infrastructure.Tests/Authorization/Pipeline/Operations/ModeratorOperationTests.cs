using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SELearning.Infrastructure.Authorization;
using SELearning.Infrastructure.Authorization.Pipeline;
using SELearning.Infrastructure.Authorization.Pipeline.Operations;

namespace SELearning.Infrastructure.Tests.Authorization.Pipeline.Operations;

public class ModeratorOperationTests
{
    private readonly IAuthorizationContextPipelineOperation _testPipelineOperation;

    public ModeratorOperationTests()
    {
        _testPipelineOperation = new ModeratorOperation();
    }

    [Fact]
    public async Task Invoke_UserWithModeratorRole_IsModeratorAddedAndSetToTrueAndUserIdAdded()
    {
        var userWithModeratorRole = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Name, "homer.simpson"),
            new(ClaimTypes.NameIdentifier, "Adrian"),
            new(ClaimTypes.Role, "Moderator"),
            new(ClaimTypes.Role, "AnotherOne")
        }));
        var context =
            new PermissionAuthorizationContext(userWithModeratorRole, Enumerable.Empty<Permission>());

        await _testPipelineOperation.Invoke(context);
        var result = context.Data.Get<bool>("IsModerator");
        var userIdResult = context.Data.Get<string>("UserId");

        Assert.True(result);
        Assert.Equal("Adrian", userIdResult);
    }

    [Fact]
    public async Task Invoke_UserWithModeratorRole_IsModeratorAddedAndSetToFalseAndUserIdAdded()
    {
        var userWithModeratorRole = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Name, "homer.simpson"),
            new(ClaimTypes.NameIdentifier, "Adrian"),
            new(ClaimTypes.Role, "AnotherOne")
        }));
        var context =
            new PermissionAuthorizationContext(userWithModeratorRole, Enumerable.Empty<Permission>());

        await _testPipelineOperation.Invoke(context);
        var result = context.Data.Get<bool>("IsModerator");
        var userIdResult = context.Data.Get<string>("UserId");

        Assert.False(result);
        Assert.Equal("Adrian", userIdResult);
    }
}
