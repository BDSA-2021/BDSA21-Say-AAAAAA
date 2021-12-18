using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

/// <summary>
/// Permission requirement for authorization
/// </summary>
/// <value></value>
public record PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(params Permission[] p) => this.Permissions = p;

    public IEnumerable<Permission> Permissions { get; init; }
}