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
        if (!TryParsePolicyPermissionsRequirement(policyName, out var requirement))
            return await DefaultProvider.GetPolicyAsync(policyName);

        var policy = new AuthorizationPolicyBuilder();
        policy.AddRequirements(requirement);

        return policy.Build();
    }

    public static string PermissionsToPolicyName<T>(params Permission[] permissions) where T : BasePermissionRequirement
        => $"{typeof(T).Name} {PermissionsToPolicyName(permissions)}";

    public static string PermissionsToPolicyName(params Permission[] permissions)
        => string.Join(
            AuthorizationConstants.POLICY_SEPERATOR,
            permissions.Select(p => $"{AuthorizationConstants.POLICY_PREFIX}{Enum.GetName(typeof(Permission), p)}")
        );

    public static bool TryParsePolicyPermissionsRequirement(string policyName, out BasePermissionRequirement parsedRequirement)
    {
        parsedRequirement = null!;

        var reqNameEndIndex = policyName.IndexOf(' ');
        if (reqNameEndIndex < 0)
            return false;

        var reqName = policyName.Substring(0, reqNameEndIndex);
        if (!policyName.StartsWith(reqName))
            return false;

        var permissionsPolicyPart = policyName.Substring(reqNameEndIndex + 1);
        if (!TryParsePolicyPermissions(permissionsPolicyPart, out var parsedPermissions))
            return false;

        switch (reqName)
        {
            case nameof(PermissionRequirement):
                parsedRequirement = new PermissionRequirement(parsedPermissions.ToArray());
                break;
            case nameof(ResourcePermissionRequirement):
                parsedRequirement = new ResourcePermissionRequirement(parsedPermissions.ToArray());
                break;
            default:
                return false;
        }

        return true;
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
}
