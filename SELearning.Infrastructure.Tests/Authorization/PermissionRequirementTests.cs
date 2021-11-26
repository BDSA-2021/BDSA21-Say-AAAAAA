using SELearning.Infrastructure.Authorization;

namespace SELearning.Infrastructure.Tests;

public class PermissionRequirementTests
{
    [Fact]
    public void PermissionGetter_GivenPermissionInConstructor_ReturnsSamePermission()
    {
        PermissionRequirement req = new(Permission.DeleteAnyComment);

        Assert.Equal(Permission.DeleteAnyComment, req.Permission);
    }
}
