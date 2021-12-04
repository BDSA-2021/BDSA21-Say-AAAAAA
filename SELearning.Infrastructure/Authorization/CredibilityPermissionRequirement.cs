using Microsoft.AspNetCore.Authorization;

namespace SELearning.Infrastructure.Authorization
{
    public record CredibilityPermissionRequirement : IAuthorizationRequirement
    {
        public int Credibility { get; }
        public CredibilityPermissionRequirement(int credScoreToHave) => Credibility = credScoreToHave;
    }
}