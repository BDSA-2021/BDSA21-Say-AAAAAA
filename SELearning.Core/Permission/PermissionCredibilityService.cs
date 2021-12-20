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
                return await Task.Run(() => -2);
            case Permission.EditAnyComment:
            case Permission.EditAnyContent:
            case Permission.DeleteAnyComment:
            case Permission.DeleteAnyContent:
            case Permission.CreateContent:
                return await Task.Run(() => 5);
            case Permission.CreateSection:
            case Permission.EditSection:
            case Permission.DeleteSection:
                return await Task.Run(() => int.MaxValue);
            case Permission.DeleteOwnComment:
            case Permission.DeleteOwnContent:
            default:
                return await Task.Run(() => int.MinValue);
        }
    }
}
