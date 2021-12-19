using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public interface IResourcePermissionService
{
    Task<AuthorizationResult> Authorize(ClaimsPrincipal user, object resource, params Permission[] permissions);
}