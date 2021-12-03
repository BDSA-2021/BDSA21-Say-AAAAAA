using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

/// <summary>
/// Creates a permission policy
/// </summary>
public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private DefaultAuthorizationPolicyProvider DefaultProvider { get; }

    private readonly IPermissionCredibilityService _permissionCredibilityService;

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options, IPermissionCredibilityService permissionCredibilityService) 
    { 
        DefaultProvider = new DefaultAuthorizationPolicyProvider(options);
        _permissionCredibilityService = permissionCredibilityService;
    }

    public async Task<AuthorizationPolicy> GetDefaultPolicyAsync() => await DefaultProvider.GetDefaultPolicyAsync();

    public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => await DefaultProvider.GetFallbackPolicyAsync();

    /// <summary>
    /// Creates a policy based on the policy name
    /// </summary>
    /// <param name="policyName">Name of the policy</param>
    /// <returns>
    /// Returns a policy based on the policy name. 
    /// If the policy name is not recognized, then it will default to Microsoft's DefaultAuthorizationPolicyProvider.GetPolicyAsync()
    /// </returns>
    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (!TryParsePolicyPermission(policyName, out Permission parsedPermission))
            return await DefaultProvider.GetPolicyAsync(policyName); // Could not parse permission... fallback to default implementation

        int requiredCredibilityScore = await _permissionCredibilityService.GetRequiredCredibility(parsedPermission);

        var policy = new AuthorizationPolicyBuilder();
        policy.AddRequirements(new CredibilityPermissionRequirement(requiredCredibilityScore));

        return policy.Build();
    }

    /// <summary>
    /// Parses the policy name to an enum of type Permission.
    /// </summary>
    /// <param name="policyName">Name of the policy</param>
    /// <param name="parsedPermission">The result of the parsed enum. If it is not able to parse, then it returns the default of the type</param>
    /// <returns>True if it is parsed and false if not</returns>
    private bool TryParsePolicyPermission(string policyName, out Permission parsedPermission)
    {
        // Permission name
        if (!policyName.StartsWith(AuthorizationConstants.POLICY_PREFIX))
        {
            parsedPermission = default(Permission);
            return false;
        }

        string permissionName = policyName.Substring(AuthorizationConstants.POLICY_PREFIX.Length);
        return Enum.TryParse<Permission>(permissionName, false, out parsedPermission);
    }
}