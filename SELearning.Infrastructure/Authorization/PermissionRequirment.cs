using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public record PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(Permission p) => this.Permission = p;

    public Permission Permission { get; init; }
}