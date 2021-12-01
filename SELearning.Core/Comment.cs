
namespace SELearning.Core
{
    public class Comment
    {
        public string? Author { get; init; }

        public DateTime Timestamp { get; init; }
        public string? Content { get; set; }

        public int Rating { get; set; } = 0;

        public int Id { get; init; }

        public int ContentId { get; init; }
    }
}