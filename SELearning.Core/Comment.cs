
namespace SELearning.Core
{   
    public class Comment
    {
        public string? Author { get; set;}   

        public DateTime Timestamp {get; init;}
        public string? Content { get; set;}

        public int Rating {get; set; } = 0;   
    }
}
