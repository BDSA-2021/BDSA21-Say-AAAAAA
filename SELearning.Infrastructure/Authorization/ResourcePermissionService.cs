using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public class ResourcePermissionService : IResourcePermissionService
{
    private readonly IAuthorizationService _authService;

    public ResourcePermissionService(IAuthorizationService authService)
    {
        _authService = authService;
    }

    public async Task<AuthorizationResult> Authorize(ClaimsPrincipal user, object resource, params Core.Permission.Permission[] permissions)
    {
        return await _authService.AuthorizeAsync(
                user,
                resource,
                $"{nameof(ResourcePermissionRequirement)} {PermissionPolicyProvider.PermissionsToPolicyName(Permission.EditAnyContent, Permission.EditOwnContent)}");
    }
}