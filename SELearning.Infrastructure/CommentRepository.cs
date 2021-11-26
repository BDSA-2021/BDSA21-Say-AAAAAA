namespace SELearning.Infrastructure
{
    public class CommentRepository: ICommentRepository
    {
        public void AddComment(string author, string content)
        {

        }
        public void UpdateComment(Comment cmt)
        {

        }
        public void RemoveComment(Comment cmt)
        {

        }

        public List<Comment> GetCommentsByContentId(int contentId)
        {
            return new List<Comment>();
        }
    }
}