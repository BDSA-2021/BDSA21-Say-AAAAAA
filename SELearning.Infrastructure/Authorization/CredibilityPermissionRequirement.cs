using Microsoft.AspNetCore.Authorization;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

public record CredibilityPermissionRequirement : IAuthorizationRequirement
{
    public IReadOnlyCollection<(Permission Permission, int Credibility)> RequiredCredibilityScores { get; }

    public CredibilityPermissionRequirement(params (Permission, int)[] credScoreToHave)
    {
        if (credScoreToHave.Length < 1)
            throw new ArgumentException("A credibility permission requirement must have at least one required credibility score");

        RequiredCredibilityScores = credScoreToHave;
    }
}