using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public static class AuthorizationConfiguration
{
    public static PermissionBuilder AddPermissionAuthorization(this IServiceCollection services)
    {
        // Inject asp net Authorization handler and policy implementations
        services.AddSingleton<IAuthorizationHandler, CredibilityAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, AuthoredCredibilityAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        return new(services);
    }
}