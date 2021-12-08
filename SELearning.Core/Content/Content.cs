namespace SELearning.Core.Content;

public class Content
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public Section? Section { get; set; }
    public string? Author { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? VideoLink { get; set; }
    public int Rating { get; set; }
}
