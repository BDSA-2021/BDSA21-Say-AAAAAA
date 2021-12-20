namespace SELearning.Core.Permission;

public enum Permission
{
    Rate,
    CreateComment,
    DeleteAnyComment,
    DeleteOwnComment,
    EditAnyComment,
    EditOwnComment,
    CreateContent,
    DeleteAnyContent,
    DeleteOwnContent,
    EditAnyContent,
    EditOwnContent,
    DeleteSection,
    EditSection,
    CreateSection
}

public static class PermissionExtensions
{
    public static bool ActsOnAuthorOnly(this Permission perm) => Enum.GetName<Permission>(perm)!.Contains("Own");
}
