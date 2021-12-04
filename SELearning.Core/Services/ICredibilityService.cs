using System.Security.Claims;

namespace SELearning.Core;

public interface ICredibilityService
{
    Task<int> GetCredibilityScore(ClaimsPrincipal user);
}