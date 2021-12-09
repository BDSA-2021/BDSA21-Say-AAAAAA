namespace SELearning.Core.Section;

public class Section
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public List<Content.Content>? Content { get; set; }

    public List<Content.Content>? GetContent()
    {
        return null;
    }
}
