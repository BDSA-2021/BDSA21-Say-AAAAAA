using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

/// <summary>
/// Permission requirement for authorization that requires one of the given
/// permissions to have been granted.
/// </summary>
public abstract record BasePermissionRequirement : IAuthorizationRequirement
{
    public BasePermissionRequirement(params Permission[] p) => this.Permissions = p;

    public IEnumerable<Permission> Permissions { get; init; }
}

public record PermissionRequirement : BasePermissionRequirement
{
    public PermissionRequirement(params Permission[] p) => Permissions = p;
}

public record ResourcePermissionRequirement : BasePermissionRequirement
{
    public ResourcePermissionRequirement(params Permission[] p) => Permissions = p;
}
