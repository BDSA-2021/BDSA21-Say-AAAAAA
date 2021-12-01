namespace SELearning.Core.Comment
{
    public record CommentCreateDTO(string? Author, string? Text, int ContentId);
    public record CommentUpdateDTO(string? Text);

    public record CommentDetailsDTO(string? Author, string? Text, int Id, DateTime Timestamp, int Rating, Content.Content Content);
}