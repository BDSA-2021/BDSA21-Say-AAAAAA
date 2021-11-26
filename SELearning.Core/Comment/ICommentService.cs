namespace SELearning.Core.Comment
{
    public interface ICommentService
    {
        void PostComment(string author, string content);
        void UpdateComment(Comment cmt);
        void RemoveComment(Comment cmt);
        void UpvoteComment(Comment cmt);
        void DownvoteComment(Comment cmt);

        List<Comment> GetCommentsFromContentId(int contentId);

    }
}