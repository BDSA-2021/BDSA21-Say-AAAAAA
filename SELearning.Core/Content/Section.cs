namespace SELearning.Core.Content;
public class Section
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public List<Content>? Content { get; set; }

    public List<Content>? GetContent()
    {
        return null;
    }
}