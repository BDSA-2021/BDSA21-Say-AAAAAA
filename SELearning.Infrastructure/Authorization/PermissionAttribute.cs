using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public class PermissionAttribute : AuthorizeAttribute
{
    public PermissionAttribute(Permission p)
    {

    }
}