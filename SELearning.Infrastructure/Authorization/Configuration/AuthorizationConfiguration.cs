using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SELearning.Core.Credibility;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public static class AuthorizationConfiguration
{
    public static PermissionBuilder AddPermissionAuthorization(this IServiceCollection services)
    {
        // Inject asp net Authorization handler and policy implementations
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, ResourcePermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddSingleton<IProvider<ICredibilityService>, Provider<ICredibilityService>>();
        services.AddScoped<IResourceAuthorizationPermissionService, ResourcePermissionService>();

        services.AddSingleton<IProvider<IPermissionCredibilityService>, Provider<IPermissionCredibilityService>>();
        services.AddSingleton<IProvider<ICredibilityService>, Provider<ICredibilityService>>();

        return new(services);
    }
}