using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public class ResourcePermissionRequirement : IAuthorizationRequirement
{
    public ResourcePermissionRequirement(params Permission[] p) => this.Permissions = p;

    public IEnumerable<Permission> Permissions { get; init; }
}