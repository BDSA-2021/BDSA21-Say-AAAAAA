using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

/// <summary>
/// Permission requirement for authorization
/// </summary>
/// <value></value>
public record PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(Permission p) => this.Permission = p;

    public Permission Permission { get; init; }
}