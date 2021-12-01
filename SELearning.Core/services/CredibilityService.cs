namespace SELearning.Core;

public interface ICredibilityService
{
    Task<int> GetCredibilityScore(User user);
}