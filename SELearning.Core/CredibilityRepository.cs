namespace SELearning.Core;

public interface ICredibilityRepository
{
    Task<int> GetContentCredibilityScore(User user);
    Task<int> GetCommentCredibilityScore(User user);
}