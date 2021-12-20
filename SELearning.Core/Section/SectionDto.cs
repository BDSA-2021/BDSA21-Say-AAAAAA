namespace SELearning.Core.Section;

public record SectionDTO
{
    public int? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}

public record SectionCreateDTO
{
    [StringLength(50)] public string? Title { get; set; }

    [StringLength(50)] public string? Description { get; set; }
}

public record SectionUpdateDTO : SectionCreateDTO;
