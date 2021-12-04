using System.Text;
using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

/// <summary>
/// Authorization attribute with the required permission
/// </summary>
public class AuthorizePermissionAttribute : AuthorizeAttribute
{
    public AuthorizePermissionAttribute(params Permission[] permissions)
    {
        if (permissions.Length < 1)
            throw new ArgumentException("A permission requirement attribute must have at least one required permission");

        StringBuilder policyNameBuilder = new();

        // Add permissions to policy name with seperator except for the last one
        foreach (Permission permission in permissions[..^1])
            policyNameBuilder.Append($"{AuthorizationConstants.POLICY_PREFIX}{Enum.GetName(typeof(Permission), permission)}{AuthorizationConstants.POLICY_SEPERATOR}");

        // Add the last permission to the policy name without seperator 
        policyNameBuilder.Append($"{AuthorizationConstants.POLICY_PREFIX}{Enum.GetName(typeof(Permission), permissions[permissions.Length - 1])}");

        Policy = policyNameBuilder.ToString();
    }
}