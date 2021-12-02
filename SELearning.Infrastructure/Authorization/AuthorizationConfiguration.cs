using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public static class AuthorizationConfiguration
{
    public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services) 
    {
        // Inject asp net Authorization handler and policy implementations
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        // Add permission service
        services.AddSingleton<IPermissionService, PermissionDecider>();

        return services;
    }
}