using SELearning.Core.Services;
using SELearning.Core;
using System;

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
            cmt.Rating++;
            UpdateComment(cmt);
        }
        public void DownvoteComment(Comment cmt)
        {
            cmt.Rating--;
            UpdateComment(cmt);
        }

        public List<Comment> GetCommentsFromContentId(int contentId)
        {
            return new List<Comment>();
        }

    }
}