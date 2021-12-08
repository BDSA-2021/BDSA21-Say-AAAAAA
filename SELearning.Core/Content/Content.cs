namespace SELearning.Core.Content;

public class Content
{
    public int Id { get; init; }
    public Section? Section { get; set; }
    public string? Author { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? VideoLink { get; set; }
    public int Rating { get; set; }
    public List<Comment.Comment> Comments { get; init; } = new();
}
