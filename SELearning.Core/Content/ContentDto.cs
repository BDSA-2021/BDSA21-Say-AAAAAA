namespace SELearning.Core.Content;
public record ContentDto {

    public int? Id { get; set; }
    public string? Section { get; set; }
    public string? Author { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? VideoLink { get; set; }
    public int Rating { get; set; }
    }

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