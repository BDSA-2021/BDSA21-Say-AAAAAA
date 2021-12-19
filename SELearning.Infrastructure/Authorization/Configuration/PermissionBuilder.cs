using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SELearning.Core.Credibility;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

/// <summary>
/// Builds the Permissions and adds them into the dependency injection system.
/// This is inspired by the AuthenticationBuilder: https://github.com/aspnet/Security/blob/master/src/Microsoft.AspNetCore.Authentication/AuthenticationBuilder.cs
/// </summary>
public class PermissionBuilder
{
    public IServiceCollection Services { get; }

    private IPolicyPipelineOperation? _pipeline;
    private Dictionary<Permission, List<IRule>> _rules = new Dictionary<Permission, List<IRule>>();
    private Dictionary<Permission, List<IResourceRule>> _resourceRules = new Dictionary<Permission, List<IResourceRule>>();

    public PermissionBuilder(IServiceCollection services)
        => Services = services;

    public PermissionBuilder AddPermissionCredibilityService(IPermissionCredibilityService service)
    {
        Services.TryAddSingleton<IPermissionCredibilityService>(service);
        return this;
    }

    public PermissionBuilder AddRule<T>(Permission p) where T : IRule, new()
    {
        if(!_rules.ContainsKey(p))
            _rules.Add(p, new List<IRule>());

        _rules[p].Add(new T());

        return this;
    }

    public PermissionBuilder AddRule<T>() where T : IRule, new()
    {
        foreach(Permission p in Enum.GetValues<Permission>())
            AddRule<T>(p);

        return this;
    }

    public PermissionBuilder AddResourceRule<T>(Permission p) where T : IResourceRule, new()
    {
        if(!_resourceRules.ContainsKey(p))
                _resourceRules.Add(p, new List<IResourceRule>());

        _resourceRules[p].Add(new T());

        return this;
    }

    public PermissionBuilder AddResourceRule<T>() where T : IResourceRule, new()
    {
        foreach(Permission p in Enum.GetValues<Permission>())
            AddResourceRule<T>(p);

        return this;
    }

    private PermissionBuilder AddPermissionPipeline(IPolicyPipelineOperation operation)
    {
        if(_pipeline == null)
            _pipeline = operation;
        else
            _pipeline.SetNext(operation);

        return this;
    }

    /// <summary>
    /// Injects the added permissions to the dependency injection system
    /// </summary>
    public void Build()
    {
        var serviceProvider = Services.BuildServiceProvider();
        AddPermissionPipeline(new ModeratorOperation());
        AddPermissionPipeline(new CredibilityOperation(serviceProvider.GetRequiredService<IProvider<IPermissionCredibilityService>>(),
                                                        serviceProvider.GetRequiredService<IProvider<ICredibilityService>>()));

                            
        Services.AddSingleton<IPolicyPipelineOperation>(_pipeline!);
        Services.AddSingleton<IPermissionService, PermissionDecider>(x => new PermissionDecider(_rules.AsEnumerable().AsEnumerable().ToDictionary(y => y.Key, z => z.Value.AsEnumerable()), _resourceRules.AsEnumerable().ToDictionary(y => y.Key, z => z.Value.AsEnumerable())));
        Services.AddSingleton<IResourcePermissionService, PermissionDecider>(x => new PermissionDecider(_rules.AsEnumerable().AsEnumerable().ToDictionary(y => y.Key, z => z.Value.AsEnumerable()), _resourceRules.AsEnumerable().ToDictionary(y => y.Key, z => z.Value.AsEnumerable())));
        Services.TryAddSingleton<IPermissionCredibilityService, PermissionCredibilityService>();
    }
}