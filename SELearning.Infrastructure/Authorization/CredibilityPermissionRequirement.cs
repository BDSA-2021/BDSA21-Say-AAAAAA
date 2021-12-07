using Microsoft.AspNetCore.Authorization;

namespace SELearning.Infrastructure.Authorization;

public record CredibilityPermissionRequirement : IAuthorizationRequirement
{
    public IReadOnlyCollection<int> RequiredCredibilityScores { get; }

    public CredibilityPermissionRequirement(params int[] credScoreToHave)
    {
        if (credScoreToHave.Length < 1)
            throw new ArgumentException("A credibility permission requirement must have at least one required credibility score");

        RequiredCredibilityScores = credScoreToHave;
    }
}