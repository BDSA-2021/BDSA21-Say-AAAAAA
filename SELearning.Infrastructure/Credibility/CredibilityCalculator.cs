using System.Security.Claims;
using SELearning.Core.Credibility;

namespace SELearning.Infrastructure.Credibility;

public class CredibilityCalculator : ICredibilityService
{
    readonly ICredibilityRepository _credibilityRepository;

    public CredibilityCalculator(ICredibilityRepository repository)
    {
        _credibilityRepository = repository;
    }

    async public Task<int> GetCredibilityScore(ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            throw new NullReferenceException("User id claim not found!");

        var commentCredibilityScore = await _credibilityRepository.GetCommentCredibilityScore(userIdClaim.Value);
        var contentCredibilityScore = await _credibilityRepository.GetContentCredibilityScore(userIdClaim.Value);

        return commentCredibilityScore + contentCredibilityScore;
    }
}