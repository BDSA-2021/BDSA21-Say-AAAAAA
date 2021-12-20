#nullable disable

namespace SELearning.Infrastructure.Comment;

public class Comment
{
    public int Id { get; init; }

    public string Text { get; set; }

    public DateTime Timestamp { get; init; }

    public int Rating { get; set; }

    public Content.Content Content { get; init; }

    public User.User Author { get; init; }

    public Comment(string text, DateTime timestamp, int rating)
    {
        Text = text;
        Timestamp = timestamp;
        Rating = rating;
    }

    public Comment(string text, DateTime? timestamp, int? rating, Content.Content content, User.User author)
    {
        Text = text;
        Timestamp = timestamp ?? DateTime.Now;
        Rating = rating ?? 0;
        Content = content;
        Author = author;
    }

    public CommentDetailsDTO ToDetailsDTO() => new(new UserDTO(Author.Id, Author.Name),
        Text, Id, Timestamp, Rating, Content.Id);
}
