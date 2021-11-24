using SELearning.Core.Services;
using SELearning.Core;

namespace SELearning.Infrastructure
{
    public class CommentManager : ICommentService
    {
        public void PostComment(string author, string content)
        {

        }
        public void UpdateComment(Comment cmt)
        {

        }
        public void RemoveComment(Comment cmt)
        {

        }
        public void UpvoteComment(Comment cmt)
        {

        }
        public void DownvoteComment(Comment cmt)
        {

        }

        public List<Comment> GetCommentsFromContentId(int contentId)
        {
            return new List<Comment>();
        }

    }
}