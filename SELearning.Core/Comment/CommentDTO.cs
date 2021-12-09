using SELearning.Core.Permission;

namespace SELearning.Core.Comment
{
    public record CommentCreateDTO(string? Author, string? Text, int ContentId);
    public record CommentUpdateDTO(string? Text, int Rating);

    public record CommentDetailsDTO(string? Author, string? Text, int Id, DateTime Timestamp, int Rating, int ContentId) : IAuthored;
}