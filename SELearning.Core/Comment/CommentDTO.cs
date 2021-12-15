using SELearning.Core.Permission;

namespace SELearning.Core.Comment
{
    public record CommentUserDTO(int ContentId, string Text);
    public record CommentCreateDTO(UserDTO Author, string? Text, int ContentId);
    public record CommentUpdateDTO(string Text, int Rating);
    public record CommentDetailsDTO(UserDTO Author, string Text, int Id, DateTime Timestamp, int Rating, int ContentId) : IAuthored;
}