namespace SELearning.Core.Credibility;

public interface ICredibilityRepository
{
    Task<int> GetContentCredibilityScore(string userId);
    Task<int> GetCommentCredibilityScore(string userId);
}
