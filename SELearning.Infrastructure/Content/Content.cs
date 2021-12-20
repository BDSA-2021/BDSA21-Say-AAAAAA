using SELearning.Core.Section;

#nullable disable
namespace SELearning.Infrastructure.Content;

public class Content
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string VideoLink { get; set; }

    public int Rating { get; set; }

    public User.User Author { get; set; }

    public Section.Section Section { get; set; }

    public Content(string title, string description, string videoLink, int rating)
    {
        Title = title;
        Description = description;
        VideoLink = videoLink;
        Rating = rating;
    }

    public Content(string title, string description, string videoLink, int? rating, User.User author,
        Section.Section section)
    {
        Title = title;
        Description = description;
        VideoLink = videoLink;
        Rating = rating ?? 0;
        Author = author;
        Section = section;
    }

    public ContentDTO ToContentDTO()
    {
        return new ContentDTO
        {
            Id = Id,
            Title = Title,
            Description = Description,
            VideoLink = VideoLink,
            Rating = Rating,
            Author = new UserDTO(Author.Id, Author.Name),
            Section = new SectionDTO
            {
                Id = Section.Id,
                Title = Section.Title,
                Description = Section.Description
            }
        };
    }
}
