namespace SELearning.Core.Content;
public class Section : IEquatable<Section>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public List<Content>? Content { get; set; }

    public List<Content>? GetContent()
    {
        return null;
    }

    public bool Equals(Section other)
        => other.Id == Id && other.Title == Title && other.Description == Description;
}