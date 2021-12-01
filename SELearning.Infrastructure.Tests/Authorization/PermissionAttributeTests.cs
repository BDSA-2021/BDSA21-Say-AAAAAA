using System;
using Microsoft.AspNetCore.Authorization;
using SELearning.Infrastructure.Authorization;
using static SELearning.Core.Permission.Permission;

namespace SELearning.Infrastructure.Tests.Authorization;

public class PermissionAttributeTests
{
    [Theory]
    [InlineData(CreateComment)]
    [InlineData(EditAnyComment)]
    [InlineData(CreateContent)]
    public void Init_WithPermission_SetsPolicyWithPrefixAndPermissionName(Permission p)
    {
        string prefix = "Permission";

        PermissionAttribute attr = new(p);

        Assert.Equal($"{prefix}{Enum.GetName(typeof(Permission), p)}", attr.Policy);
    }
}