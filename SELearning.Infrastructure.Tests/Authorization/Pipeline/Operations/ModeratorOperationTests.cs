using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SELearning.Infrastructure.Authorization;


public class ModeratorOperationTests
{
    private IAuthorizationContextPipelineOperation _testPipelineOperation;
    public ModeratorOperationTests()
    {
        _testPipelineOperation = new ModeratorOperation();
    }

    [Fact]
    public async Task Invoke_UserWithModeratorRole_IsModeratorAddedAndSetToTrueAndUserIdAdded()
    {
        var userWithModeratorRole = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson"), new Claim(ClaimTypes.NameIdentifier, "Adrian"), new Claim(ClaimTypes.Role, "Moderator"), new Claim(ClaimTypes.Role, "AnotherOne") }));
        PermissionAuthorizationContext context = new PermissionAuthorizationContext(userWithModeratorRole, Enumerable.Empty<Permission>());

        await _testPipelineOperation.Invoke(context);
        bool result = context.Data.Get<bool>("IsModerator");
        string userIdResult = context.Data.Get<string>("UserId");

        Assert.True(result);
        Assert.Equal("Adrian", userIdResult);
    }

    [Fact]
    public async Task Invoke_UserWithModeratorRole_IsModeratorAddedAndSetToFalseAndUserIdAdded()
    {
        var userWithModeratorRole = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson"), new Claim(ClaimTypes.NameIdentifier, "Adrian"), new Claim(ClaimTypes.Role, "AnotherOne") }));
        PermissionAuthorizationContext context = new PermissionAuthorizationContext(userWithModeratorRole, Enumerable.Empty<Permission>());

        await _testPipelineOperation.Invoke(context);
        bool result = context.Data.Get<bool>("IsModerator");
        string userIdResult = context.Data.Get<string>("UserId");

        Assert.False(result);
        Assert.Equal("Adrian", userIdResult);
    }
}