using System.Security.Claims;

namespace SELearning.Core.Credibility;

public interface ICredibilityService
{
    Task<int> GetCredibilityScore(ClaimsPrincipal user);
}
