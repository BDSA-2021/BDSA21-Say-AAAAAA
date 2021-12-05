namespace SELearning.Core.Permission;

public class PermissionCredibilityService : IPermissionCredibilityService
{
    public async Task<int> GetRequiredCredibility(Permission p)
    {
        switch (p)
        {
            case Permission.EditOwnComment:
            case Permission.EditOwnContent:
            case Permission.CreateComment:
            case Permission.Rate:
                return await Task.Run<int>(() => -10);
            case Permission.EditAnyComment:
            case Permission.EditAnyContent:
            case Permission.DeleteAnyComment:
            case Permission.DeleteAnyContent:
            case Permission.CreateContent:
                return await Task.Run<int>(() => 1000);

            default:
                return await Task.Run<int>(() => int.MinValue);
        }
    }
}