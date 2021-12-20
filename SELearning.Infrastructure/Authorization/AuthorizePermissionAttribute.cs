using System.Text;
using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

/// <summary>
/// Authorization attribute with the required permission.
/// </summary>
/// <remarks>
/// When adding mulitple permissions to this attribute, the permission will be evaluted
/// as 'or' - that is, authorization is granted if the user has at least one of the
/// specified permissions. In order to have the permissions to be evaluated as 'and'
/// (i.e. the user should have all of the specified permissions), you have to add
/// multiple <c>AuthorizePermissionAttribute</c>s with the required permission.
/// </remarks>
/// <example>
/// This is an example of 'or' evaluated permissions (<c>Permission.EditAnyComment</c>
/// OR <c>Permission.EditOwnComment</c>):
/// <code>
///   [AuthorizePermission(Permission.EditAnyComment, Permission.EditOwnComment)]
///   public Task MyControllerMethod()
/// </code>
/// This is an example of 'and' evaluated permissions (<c>Permission.EditAnyComment</c>
/// AND <c>Permission.EditOwnComment</c>):
/// <code>
///   [AuthorizePermission(Permission.EditAnyComment)]
///   [AuthorizePermission(Permission.EditOwnComment)]
///   public Task MyControllerMethod()
/// </code>
/// <seealso cref="SELearning.Core.Permission" />
/// </example>
public class AuthorizePermissionAttribute : AuthorizeAttribute
{
    public AuthorizePermissionAttribute(params Permission[] permissions)
    {
        if (permissions.Length < 1)
            throw new ArgumentException("A permission requirement attribute must have at least one required permission");

        Policy = PermissionPolicyProvider.PermissionsToRequirementPolicyName<PermissionRequirement>(permissions);
    }
}