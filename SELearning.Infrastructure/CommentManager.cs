using SELearning.Core.Comment;
using SELearning.Core;
using System;


namespace SELearning.Infrastructure
{

    public class CommentManager : ICommentService
    {
        ICommentRepository _repo;
        public CommentManager(ICommentRepository repo){
            _repo = repo;
        }
        public void PostComment(CommentCreateDTO dto)
        {
            if(_repo.AddComment(dto).Result.Item1 == OperationResult.NotFound)
            {
                throw new Exception("The content that the comment belongs to could not be found");    
            }
        }
        public void UpdateComment(int id, CommentUpdateDTO dto)
        {
            if(_repo.UpdateComment(id, dto).Result.Item1 == OperationResult.NotFound)
            {
                throw new Exception("The comment could not be found");
            }   
        }
        public void RemoveComment(int id)
        {
            if(_repo.RemoveComment(id).Result == OperationResult.NotFound)
            {
                throw new Exception("The comment could not be found");
            }     
        }
        public void UpvoteComment(int id)
        {
            Comment comment = _repo.GetCommentByCommentId(id).Result;
            comment.Rating++;
            CommentUpdateDTO dto = new CommentUpdateDTO(comment.Text,comment.Rating);
            UpdateComment(id,dto);
        }
        public void DownvoteComment(int id)
        {
            Comment comment = _repo.GetCommentByCommentId(id).Result;
            comment.Rating--;
            CommentUpdateDTO dto = new CommentUpdateDTO(comment.Text,comment.Rating);
            UpdateComment(id,dto);
        }

        public List<Comment> GetCommentsFromContentId(int contentId)
        {
            var comments = _repo.GetCommentsByContentId(contentId).Result;
            if(comments.Item2 == OperationResult.NotFound)
            {
                throw new Exception("The content that the comments belongs to could not be found");    
            }
            return comments.Item1;
        }

        public Comment GetCommentFromCommentId(int commentId){
            return _repo.GetCommentByCommentId(commentId).Result;
        }

    }
}