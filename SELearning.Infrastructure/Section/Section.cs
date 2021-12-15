using SELearning.Core.Section;

namespace SELearning.Infrastructure.Section;

public class Section : IEquatable<Section>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public List<Content.Content>? Content { get; set; }

    public List<Content.Content>? GetContent()
    {
        return null;
    }

    public bool Equals(Section? other)
    {
        if (other == null) return false;

        return other.Id == Id && other.Title == Title && other.Description == Description;
    }

    public override bool Equals(object? other)
    {
        return Equals(other as Section);
    }

    public SectionDTO ToSectionDTO() => new SectionDTO { Id = Id, Title = Title, Description = Description };
}
