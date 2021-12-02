using System.Security.Claims;

namespace SELearning.Infrastructure;

public class CredibilityCalculator : ICredibilityService
{
    readonly ICredibilityRepository _credibilityRepository;

    public CredibilityCalculator(ICredibilityRepository repository)
    {
        _credibilityRepository = repository;
    }

    async public Task<int> GetCredibilityScore(ClaimsPrincipal user)
    {
        string userId = user.FindFirst(ClaimTypes.NameIdentifier).Value;

        var commentCredibilityScore = _credibilityRepository.GetCommentCredibilityScore(userId);
        var contentCredibilityScore = _credibilityRepository.GetContentCredibilityScore(userId);
        var scores = new[] { commentCredibilityScore, contentCredibilityScore };

        return (await Task.WhenAll(scores)).Sum();
    }
}