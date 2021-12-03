using Microsoft.AspNetCore.Authorization;

namespace SELearning.Infrastructure.Authorization
{
    public class CredibilityPermissionRequirement : IAuthorizationRequirement
    {
        public int Credibility { get; }
        public CredibilityPermissionRequirement(int credScoreToHave)
        {
            Credibility = credScoreToHave;
        }
    }
}