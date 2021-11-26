using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private DefaultAuthorizationPolicyProvider DefaultProvider { get; }
    private AuthorizationOptions Options { get; }

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        Options = options.Value;
        DefaultProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public async Task<AuthorizationPolicy> GetDefaultPolicyAsync() => await DefaultProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => Task.FromResult<AuthorizationPolicy?>(null);

    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName) 
    {
        if(!TryParsePolicyPermission(policyName, out Permission parsedPermission))
            return await DefaultProvider.GetPolicyAsync(policyName); // Could not parse permission... fallback to default implementation

        var policy = new AuthorizationPolicyBuilder();
        policy.AddRequirements(new PermissionRequirement(parsedPermission));

        return policy.Build();
    }

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