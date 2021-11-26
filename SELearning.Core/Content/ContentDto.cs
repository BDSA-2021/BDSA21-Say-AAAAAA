namespace SELearning.Core.Content;
public record ContentDto(
    int? Id,
    string? Section,
    string? Author,
    string? Title,
    string? Description,
    string? VideoLink,
    int Rating
    );

public record ContentCreateDto
{
    [StringLength(50)]
    public string? Section { get; init; }

    [StringLength(50)]
    public string? Author { get; init; }

    [StringLength(50)]
    public string? Title { get; init; }

    [StringLength(50)]
    public string? Description { get; init; }

    [StringLength(50)]
    public string? VideoLink { get; init; }

    public int Rating { get; init; }

}

public record ContentUpdateDto : ContentCreateDto
{
    public int Id { get; init; }
}