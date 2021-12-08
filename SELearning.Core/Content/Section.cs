namespace SELearning.Core.Content;
public class Section
{
    public int Id { get; init; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public List<Content> Content { get; set; } = new();
}