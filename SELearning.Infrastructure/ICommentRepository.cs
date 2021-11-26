namespace SELearning.Infrastructure
{
    public interface ICommentRepository
    {
        Task<OperationResult> AddComment(Comment cmt);
        Task<OperationResult> UpdateComment(int Id, Comment cmt);
        Task<OperationResult> RemoveComment(int Id);

        Task<List<Comment>> GetCommentsByContentId(int contentId);
    }
}