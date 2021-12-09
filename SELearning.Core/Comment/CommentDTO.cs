namespace SELearning.Core.Comment
{
    public record CommentUserDTO(int ContentId, string Text);
    public record CommentCreateDTO(User.User Author, string? Text, int ContentId);
    public record CommentUpdateDTO(string? Text, int Rating);

    public record CommentDetailsDTO(User.User Author, string? Text, int Id, DateTime Timestamp, int Rating, int ContentId);
}