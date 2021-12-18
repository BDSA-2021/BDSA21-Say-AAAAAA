using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SELearning.Core.Credibility;

namespace SELearning.Infrastructure.Authorization;

public class CredibilityOperationTests
{
    private BasePipelineOperation _testPipelineOperation;
    private ClaimsPrincipal _user;
    public CredibilityOperationTests()
    {
        var permissionService = new Mock<ICredibilityService>();
        permissionService.Setup(m => m.GetCredibilityScore(_user)).ReturnsAsync(1000);

        var provider = new Mock<IProvider<ICredibilityService>>();
        provider.Setup(x => x.Get()).Returns(permissionService.Object);
        
        _testPipelineOperation = new CredibilityOperation(null, provider.Object);
        _user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson"), new Claim(ClaimTypes.Role, "Moderator"), new Claim(ClaimTypes.Role, "AnotherOne") }));
    }

    [Fact]
    public async Task Invoke_ContextWithNoRequestedPermission_AnEmptyEnumerableWithNoItemsIsAddedToData()
    {
        
        PermissionAuthorizationContext context = new PermissionAuthorizationContext(_user, Enumerable.Empty<Permission>());

        await _testPipelineOperation.Invoke(context);
        IEnumerable<int> result = context.Data.Get<IEnumerable<int>>("RequiredCredibilityScore");

        Assert.Equal(Enumerable.Empty<int>(), result);
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