using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Collections;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

/// <summary>
/// Permission requirement for authorization
/// </summary>
/// <value></value>
public record PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(IDynamicDictionary data) => this.Data = data;

    public IDynamicDictionary Data { get; init; }
}