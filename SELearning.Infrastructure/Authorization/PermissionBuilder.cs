using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

/// <summary>
/// Builds the Permissions and adds them into the dependency injection system.
/// This is inspired by the AuthenticationBuilder: https://github.com/aspnet/Security/blob/master/src/Microsoft.AspNetCore.Authentication/AuthenticationBuilder.cs
/// </summary>
public class PermissionBuilder
{
    public IServiceCollection Services { get; }

    public PermissionBuilder(IServiceCollection services)
        => Services = services;

    public PermissionBuilder AddPermissionCredibilityService(IPermissionCredibilityService service)
    {
        Services.TryAddSingleton<IPermissionCredibilityService>(service);
        return this;
    }

    /// <summary>
    /// Injects the added permissions to the dependency injection system
    /// </summary>
    public void Build()
    {
        Services.TryAddSingleton<IPermissionCredibilityService, PermissionCredibilityService>();
    }
}