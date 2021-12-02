using Microsoft.Extensions.DependencyInjection;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public class PermissionBuilder
{
    public IServiceCollection Services { get; }

    private Dictionary<Permission, IList<Rule>> _permissions = new();

    public PermissionBuilder(IServiceCollection services) 
        => Services = services;

    public void AddRule(Permission permissionKey, Rule rule)
    {
        if(!_permissions.ContainsKey(permissionKey))
            _permissions[permissionKey] = new List<Rule>();

        _permissions[permissionKey].Add(rule);
    }

    public void Build()
    {
        Services.AddSingleton<IDictionary<Permission, IEnumerable<Rule>>>(
            x => (IDictionary<Permission, IEnumerable<Rule>>)_permissions);
    }
}