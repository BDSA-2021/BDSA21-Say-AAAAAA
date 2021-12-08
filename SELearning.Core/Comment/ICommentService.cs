namespace SELearning.Core.Comment
{
    public interface ICommentService
    {
        Task<CommentDetailsDTO> PostComment(CommentCreateDTO dto);
        Task UpdateComment(int id, CommentUpdateDTO dto);
        Task RemoveComment(int id);
        Task UpvoteComment(int id);
        Task DownvoteComment(int id);
        Task<IEnumerable<CommentDetailsDTO>> GetCommentsFromContentId(int contentId);
        Task<CommentDetailsDTO> GetCommentFromCommentId(int commentId);
        Task<IEnumerable<CommentDetailsDTO>> GetCommentsByAuthor(string userId);
    }
}
