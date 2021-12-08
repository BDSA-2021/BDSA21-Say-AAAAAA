namespace SELearning.Core.Comment
{
    public interface ICommentService
    {
        Task PostComment(CommentCreateDTO dto);
        Task UpdateComment(int id, CommentUpdateDTO dto);
        Task RemoveComment(int id);
        Task UpvoteComment(int id);
        Task DownvoteComment(int id);
        Task<List<Comment>> GetCommentsFromContentId(int contentId);
        Task<Comment> GetCommentFromCommentId(int commentId);
    }
}
