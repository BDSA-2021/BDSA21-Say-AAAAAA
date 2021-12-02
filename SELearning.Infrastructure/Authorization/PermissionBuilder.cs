using Microsoft.Extensions.DependencyInjection;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

/// <summary>
/// Builds the Permissions and adds them into the dependency injection system
/// </summary>
public class PermissionBuilder
{
    public IServiceCollection Services { get; }

    private Dictionary<Permission, IList<Rule>> _permissions = new();

    public PermissionBuilder(IServiceCollection services) 
        => Services = services;

    /// <summary>
    /// Adds a rule to permission
    /// </summary>
    /// <param name="permissionKey">Permission to add the rule to</param>
    /// <param name="rule">Rule to add to the permission</param>
    /// <returns>Returns the current builder</returns>
    public PermissionBuilder AddRule(Permission permissionKey, Rule rule)
    {
        if(!_permissions.ContainsKey(permissionKey))
            _permissions[permissionKey] = new List<Rule>();

        _permissions[permissionKey].Add(rule);

        return this;
    }

    /// <summary>
    /// Injects the add permissions to the dependency injection system
    /// </summary>
    public void Build()
    {
        Services.AddSingleton<IDictionary<Permission, IEnumerable<Rule>>>(
            x => (IDictionary<Permission, IEnumerable<Rule>>)_permissions);
    }
}