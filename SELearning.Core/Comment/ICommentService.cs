namespace SELearning.Core.Comment
{
    public interface ICommentService
    {
        void PostComment(CommentCreateDTO dto);
        void UpdateComment(int id, CommentUpdateDTO dto);
        void RemoveComment(int id);
        void UpvoteComment(int id);
        void DownvoteComment(int id);

        List<Comment> GetCommentsFromContentId(int contentId);

        Comment GetCommentFromCommentId(int commentId);

    }
}