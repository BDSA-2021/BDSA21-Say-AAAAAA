
namespace SELearning.Core.Comment
{
    public class Comment
    {
        public string? Author { get; set; }

        public DateTime Timestamp { get; init; } = DateTime.Now;
        public string? Text { get; set; }

        public int Rating { get; set; } = 0;

        public int Id { get; init; }

        public Content.Content Content { get; init; } = default!;
    }
}
