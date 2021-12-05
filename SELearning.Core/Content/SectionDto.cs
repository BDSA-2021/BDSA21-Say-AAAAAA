namespace SELearning.Core.Content;
public record SectionDto
{

    public int? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public List<Content>? Content { get; set; }

}

public record SectionCreateDto
{
    [StringLength(50)]
    public string? Title { get; set; }

    [StringLength(50)]
    public string? Description { get; set; }

    public List<Content>? Content { get; set; }

}

public record SectionUpdateDto : SectionCreateDto
{
    public int Id { get; init; }
}