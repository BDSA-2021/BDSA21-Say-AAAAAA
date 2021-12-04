namespace SELearning.Core.Permission;

public interface IPermissionCredibilityService
{
    Task<int> GetRequiredCredibility(Permission p);
}