using System;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using SELearning.Infrastructure.Authorization;
using static SELearning.Core.Permission.Permission;

namespace SELearning.Infrastructure.Tests.Authorization;

public class PermissionAttributeTests
{
    [Theory]
    [InlineData("PermissionCreateComment", CreateComment)]
    [InlineData("PermissionEditAnyComment", EditAnyComment)]
    [InlineData("PermissionCreateContent", CreateContent)]
    [InlineData("PermissionCreateComment OR EditAnyComment", CreateComment, EditAnyComment)]
    [InlineData("PermissionEditAnyComment OR Rate OR DeleteAnyContent", EditAnyComment, Rate, DeleteAnyContent)]
    public void Init_WithPermission_SetsPolicyWithPrefixAndPermissionName(string expectedPolicyName, params Permission[] p)
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