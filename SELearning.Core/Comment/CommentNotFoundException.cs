namespace SELearning.Core
{
    public class CommentNotFoundException : NotFoundException
    {
        public CommentNotFoundException(int id)
            : base("Comment with id could not be found. ID: " + id)
        {
        }
    }
}