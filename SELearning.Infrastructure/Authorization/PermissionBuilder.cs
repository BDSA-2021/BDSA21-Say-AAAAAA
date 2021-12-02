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
    /// Adds a credibility rule to the permission
    /// </summary>
    /// <param name="permissionKey">Permission to add a credibility rule to</param>
    /// <param name="requiredCredibilityScore">The required credibility score that the user needs to have</param>
    /// <returns>Returns the current builder</returns>
    public PermissionBuilder AddCredibilityRule(Permission permissionKey, int requiredCredibilityScore)
    {
        /* NOTE: I am still not sure if this is a good solution. 
        *  We loose dependency injection here by creating concrete instances of the credibility calculator.
        *  What could make this better is by having a factory class for ICredibilityService and ICredibilityRepository, 
        *  and then later use them as the way to create instances in dependency injection?
        *
        *  Or else i have another solution, where we get rid of the PermissionDecider and instead lets the AuthorizationHandler handle it
        *  by providing a Requirement with the cred score that the user has to be above...
        */ 

        ICredibilityRepository credibilityRepository = null; // TODO: Add Cred repo here
        ICredibilityService credibilityService = new CredibilityCalculator(credibilityRepository!);

        return AddRule(permissionKey, async x => await credibilityService.GetCredibilityScore(x) >= requiredCredibilityScore);
    }

    /// <summary>
    /// Injects the added permissions to the dependency injection system
    /// </summary>
    public void Build()
    {
        Services.TryAddSingleton<IDictionary<Permission, IEnumerable<Rule>>>(
            x => (IDictionary<Permission, IEnumerable<Rule>>)_permissions);
    }
}