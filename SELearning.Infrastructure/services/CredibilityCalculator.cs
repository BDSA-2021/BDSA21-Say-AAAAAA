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
        var commentCredibilityScore = await _credibilityRepository.GetCommentCredibilityScore(user);
        var contentCredibilityScore = await _credibilityRepository.GetContentCredibilityScore(user);

        return commentCredibilityScore + contentCredibilityScore;
    }
}