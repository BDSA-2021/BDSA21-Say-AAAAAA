using SELearning.Core.Collections;
using SELearning.Infrastructure.Authorization;

namespace SELearning.Infrastructure.Tests;

public class PermissionRequirementTests
{
    [Fact]
    public void PermissionGetter_GivenPermissionDataInConstructor_ReturnsSamePermissionData()
    {
        var dictionary = new DynamicDictionary();

        PermissionRequirement req = new(dictionary);

        Assert.Equal(dictionary, req.Data);
    }
}
