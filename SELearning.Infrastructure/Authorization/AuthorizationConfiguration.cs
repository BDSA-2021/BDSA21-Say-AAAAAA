using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public static class AuthorizationConfiguration
{
    public static PermissionBuilder AddPermissionAuthorization(this IServiceCollection services)
    {
        return new(services);
    }
}