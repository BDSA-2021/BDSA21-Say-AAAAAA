using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace SELearning.Infrastructure.Authorization;

public static class AuthorizationConfiguration
{
    public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services) 
    {
        // Inject asp net Authorization handler and policy implementations
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        

        return services;
    }
}