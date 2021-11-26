namespace SELearning.Core.Comment
{
    public record CommentCreateDTO(string ?Author, string ?Content);
    public record CommentDetailsDTO(string ?Author, string ?Content, int Id, DateTime Timestamp, int Rating, int ContentId);
}