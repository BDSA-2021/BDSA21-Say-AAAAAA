using static SELearning.Core.Permission.Permission;

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
    // TODO: This is an UGLY hack. We need to refactor, but you know what they
    // say: make it work, then refactor
    public static bool ActsOnAuthorOnly(this Permission perm) => Enum.GetName<Permission>(perm)!.Contains("Own");
}