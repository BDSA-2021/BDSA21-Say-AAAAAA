#nullable disable
using SELearning.Core.Permission;

namespace SELearning.Core.Content;

public record ContentUserDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string VideoLink { get; set; }
    public string SectionId { get; set; }
}

public record ContentDto : IAuthored
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string VideoLink { get; set; }

    public int Rating { get; set; }

    public User.User Author { get; set; }

    public Section Section { get; set; }
}

public record ContentUserDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? VideoLink { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string? SectionId { get; set; }
}

public record ContentCreateDto
{
    [Required]
    public int SectionId { get; init; }
    
    [StringLength(50)]
    public string Title { get; init; }

    [StringLength(50)]
    public string Description { get; init; }

    [StringLength(50)]
    public string VideoLink { get; init; }

    [StringLength(50)]
    public User.User Author { get; init; }


}

public record ContentUpdateDto
{
    [StringLength(50)]
    public string Title { get; init; }

    [StringLength(50)]
    public string Description { get; init; }

    [StringLength(50)]
    public string VideoLink { get; init; }
    public int Rating { get; init; }
}
