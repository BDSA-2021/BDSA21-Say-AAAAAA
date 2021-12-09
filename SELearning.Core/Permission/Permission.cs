using static SELearning.Core.Permission.Permission;

namespace SELearning.Core.Permission;


public enum Permission
{
    Rate,
    CreateComment,
    DeleteAnyComment,
    EditAnyComment,
    EditOwnComment,
    CreateContent,
    DeleteAnyContent,
    EditAnyContent,
    EditOwnContent,
    DeleteOwnSection,
    DeleteAnySection,
    EditAnySection,
    EditOwnSection,
    CreateSection
}

public static class PermissionExtensions
{
    // TODO: This is an UGLY hack. We need to refactor, but you know what they
    // say: make it work, then refactor
    public static bool ActsOnAuthorOnly(this Permission perm) => Enum.GetName<Permission>(perm)!.Contains("Own");
}