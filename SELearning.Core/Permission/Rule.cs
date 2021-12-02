using System.Security.Claims;

namespace SELearning.Core.Permission;

public delegate Task<bool> Rule(ClaimsPrincipal user);