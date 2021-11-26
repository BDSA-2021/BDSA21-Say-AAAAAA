namespace SELearning.Infrastructure;

public class CredibilityCalculator : ICredibilityService
{
    readonly ICredibilityRepository _credibilityRepository;

    public CredibilityCalculator(ICredibilityRepository repository)
    {
        _credibilityRepository = repository;
    }

    async public Task<int> GetCredibilityScore(User user)
    {
        var commentCredibilityScore = _credibilityRepository.GetCommentCredibilityScore(user);
        var contentCredibilityScore = _credibilityRepository.GetContentCredibilityScore(user);
        var scores = new[] { commentCredibilityScore, contentCredibilityScore };

        return (await Task.WhenAll(scores)).Sum();
    }
}