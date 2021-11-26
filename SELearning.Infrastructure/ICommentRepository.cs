namespace SELearning.Infrastructure
{
    public interface ICommentRepository
    {
        void AddComment(string author, string content);
        void UpdateComment(Comment cmt);
        void RemoveComment(Comment cmt);

        List<Comment> GetCommentsByContentId(int contentId);
    }
}