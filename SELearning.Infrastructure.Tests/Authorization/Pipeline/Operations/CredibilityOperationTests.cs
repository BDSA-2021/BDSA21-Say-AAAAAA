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
        permissionService.Setup(m => m.GetCredibilityScore(It.IsNotNull<ClaimsPrincipal>())).ReturnsAsync(1000);

        var permissionCredibilityService = new Mock<IPermissionCredibilityService>();
        permissionCredibilityService.Setup(m => m.GetRequiredCredibility(It.IsNotNull<Permission>())).ReturnsAsync(500);

        var permissionCredServiceProvider = new Mock<IProvider<IPermissionCredibilityService>>();
        permissionCredServiceProvider.Setup(x => x.Get()).Returns(permissionCredibilityService.Object);

        var credServiceProvider = new Mock<IProvider<ICredibilityService>>();
        credServiceProvider.Setup(x => x.Get()).Returns(permissionService.Object);
        
        _testPipelineOperation = new CredibilityOperation(permissionCredServiceProvider.Object, credServiceProvider.Object);
        _user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "homer.simpson"), new Claim(ClaimTypes.Role, "Moderator"), new Claim(ClaimTypes.Role, "AnotherOne") }));
    }

    [Fact]
    public async Task Invoke_ContextWithNoRequestedPermission_AnEmptyEnumerableWithNoItemsIsAddedToData()
    {
        
        PermissionAuthorizationContext context = new PermissionAuthorizationContext(_user, Enumerable.Empty<Permission>());

        await _testPipelineOperation.Invoke(context);

        IReadOnlyDictionary<Permission, int> resultCredScores = context.Data.Get<IReadOnlyDictionary<Permission, int>>("RequiredCredibilityScores");
        int resultUserScore = context.Data.Get<int>("UserCredibilityScore");

        Assert.Equal(1000, resultUserScore);
        Assert.Equal(new Dictionary<Permission, int>(), resultCredScores);
    }

    [Fact]
    public async Task Invoke_WithRequiredPermissionsAndUser_ReturnsRequiredCredAndUserCredFromContextData()
    {
        PermissionAuthorizationContext context = new PermissionAuthorizationContext(_user, new List<Permission>{Permission.CreateComment, Permission.CreateContent});

        await _testPipelineOperation.Invoke(context);

        IReadOnlyDictionary<Permission, int> resultCredScores = context.Data.Get<IReadOnlyDictionary<Permission, int>>("RequiredCredibilityScores");
        int resultUserScore = context.Data.Get<int>("UserCredibilityScore");

        Assert.Equal(1000, resultUserScore);
        Assert.Equal(new Dictionary<Permission, int>(){ {Permission.CreateComment, 500}, {Permission.CreateContent, 500}}, resultCredScores);
    }
}