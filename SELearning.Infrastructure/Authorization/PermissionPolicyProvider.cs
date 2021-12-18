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
        var requiredScores = new List<(Permission, int)>();
        foreach (var permissionName in policyName.Split(AuthorizationConstants.POLICY_SEPERATOR))
        {
            if (!TryParsePolicyPermission(permissionName, out Permission parsedPermission))
                return await DefaultProvider.GetPolicyAsync(permissionName); // Could not parse permission... fallback to default implementation

            int requiredCredibilityScore = await _permissionCredibilityService.GetRequiredCredibility(parsedPermission);
            requiredScores.Add(new(parsedPermission, requiredCredibilityScore));
        }

        var policy = new AuthorizationPolicyBuilder();
        policy.AddRequirements(new CredibilityPermissionRequirement(requiredScores.ToArray()), new PermissionRequirement(requiredScores.Select(x => x.Item1).ToArray()));

        return policy.Build();
    }

    /// <summary>
    /// Parses the policy name (containing a single permission) to an enum of type Permission.
    /// </summary>
    /// <param name="policyName">Name of the policy</param>
    /// <param name="parsedPermission">The result of the parsed enum. If it is not able to parse, then it returns the default of the type</param>
    /// <returns>True if it is parsed and false if not</returns>
    public static bool TryParsePolicyPermission(string policyName, out Permission parsedPermission)
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

    /// <summary>
    /// Parses the policy name (containing multiple permissions) to a list of Permission enum instances.
    /// </summary>
    public static bool TryParsePolicyPermissions(string policyName, out IEnumerable<Permission> parsedPermissions)
    {
        var policyParts = policyName.Split(AuthorizationConstants.POLICY_SEPERATOR);
        var permissions = new List<Permission>();
        parsedPermissions = permissions;

        if (policyName.Length == 0)
            return true;

        foreach (var policyPart in policyParts)
        {
            if (TryParsePolicyPermission(policyPart, out var permission))
                permissions.Add(permission);
            else
                return false;
        }
        return true;
    }

    public static string PermissionsToPolicyName(params Permission[] permissions)
        => string.Join(
            AuthorizationConstants.POLICY_SEPERATOR,
            permissions.Select(p => $"{AuthorizationConstants.POLICY_PREFIX}{Enum.GetName(typeof(Permission), p)}")
        );
}