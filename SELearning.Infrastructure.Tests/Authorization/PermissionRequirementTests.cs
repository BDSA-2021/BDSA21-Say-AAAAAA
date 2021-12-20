using SELearning.Infrastructure.Authorization;

namespace SELearning.Infrastructure.Tests.Authorization;

public class PermissionRequirementTests
{
    [Fact]
    public void PermissionGetter_GivenPermissionInConstructor_ReturnsSamePermission()
    {
        PermissionRequirement req = new(Permission.DeleteAnyComment);

        Assert.Equal(new List<Permission> { Permission.DeleteAnyComment }, req.Permissions);
    }
}
