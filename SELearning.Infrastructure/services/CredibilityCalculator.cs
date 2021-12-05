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
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            throw new NullReferenceException("User id claim not found!");

        var commentCredibilityScore = _credibilityRepository.GetCommentCredibilityScore(userIdClaim.Value);
        var contentCredibilityScore = _credibilityRepository.GetContentCredibilityScore(userIdClaim.Value);
        var scores = new[] { commentCredibilityScore, contentCredibilityScore };

        return (await Task.WhenAll(scores)).Sum();
    }
}