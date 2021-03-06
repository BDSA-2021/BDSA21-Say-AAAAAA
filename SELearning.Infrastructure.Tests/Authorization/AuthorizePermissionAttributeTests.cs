using System;
using SELearning.Infrastructure.Authorization;
using static SELearning.Core.Permission.Permission;

namespace SELearning.Infrastructure.Tests.Authorization;

public class PermissionAttributeTests
{
    [Theory]
    [InlineData("PermissionRequirement PermissionCreateComment", CreateComment)]
    [InlineData("PermissionRequirement PermissionEditAnyComment", EditAnyComment)]
    [InlineData("PermissionRequirement PermissionCreateContent", CreateContent)]
    [InlineData("PermissionRequirement PermissionCreateComment OR PermissionEditAnyComment", CreateComment,
        EditAnyComment)]
    [InlineData("PermissionRequirement PermissionEditAnyComment OR PermissionRate OR PermissionDeleteAnyContent",
        EditAnyComment, Rate, DeleteAnyContent)]
    public void Init_WithPermission_SetsPolicyWithPrefixAndPermissionName(string expectedPolicyName,
        params Permission[] p)
    {
        AuthorizePermissionAttribute attr = new(p);

        Assert.Equal(expectedPolicyName, attr.Policy);
    }

    [Fact]
    public void Init_WithoutPermissions_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new AuthorizePermissionAttribute());
    }
}
