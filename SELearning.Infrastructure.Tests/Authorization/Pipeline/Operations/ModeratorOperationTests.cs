using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SELearning.Infrastructure.Authorization;


public class ModeratorOperationTests
{
    private BasePipelineOperation _testPipelineOperation;
    public ModeratorOperationTests()
    {
        _testPipelineOperation = new ModeratorOperation();
    }

    [Fact]
    public async Task Invoke_UserWithModeratorRole_IsModeratorAddedAndSetToTrue()
    {
        var userWithModeratorRole = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson"), new Claim(ClaimTypes.Role, "Moderator"), new Claim(ClaimTypes.Role, "AnotherOne") }));
        PermissionAuthorizationContext context = new PermissionAuthorizationContext(userWithModeratorRole, Enumerable.Empty<Permission>());

        await _testPipelineOperation.Invoke(context);
        bool result = context.Data.Get<bool>("IsModerator");

        Assert.True(result);
    }

    [Fact]
    public async Task Invoke_UserWithModeratorRole_IsModeratorAddedAndSetToFalse()
    {
        var userWithModeratorRole = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson"), new Claim(ClaimTypes.Role, "AnotherOne") }));
        PermissionAuthorizationContext context = new PermissionAuthorizationContext(userWithModeratorRole, Enumerable.Empty<Permission>());

        await _testPipelineOperation.Invoke(context);
        bool result = context.Data.Get<bool>("IsModerator");

        Assert.False(result);
    }
}