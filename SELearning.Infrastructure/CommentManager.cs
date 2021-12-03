using SELearning.Core.Comment;

namespace SELearning.Infrastructure
{
    public class CommentManager : ICommentService
    {
        ICommentRepository _repo;
        public CommentManager(ICommentRepository repo)
        {
            _repo = repo;
        }

        public async Task PostComment(CommentCreateDTO dto)
        {
            if ((await _repo.AddComment(dto)).Item1 == OperationResult.NotFound)
            {
                throw new Exception("The content that the comment belongs to could not be found");
            }
        }

        public async Task UpdateComment(int id, CommentUpdateDTO dto)
        {
            if ((await _repo.UpdateComment(id, dto)).Item1 == OperationResult.NotFound)
            {
                throw new Exception("The comment could not be found");
            }
        }

        public async Task RemoveComment(int id)
        {
            if (await _repo.RemoveComment(id) == OperationResult.NotFound)
            {
                throw new Exception("The comment could not be found");
            }
        }

        public async Task UpvoteComment(int id)
        {
            var comment = await _repo.GetCommentByCommentId(id);

            if (comment.IsNone)
            {
                throw new Exception("The comment could not be found");
            }

            CommentUpdateDTO dto = new CommentUpdateDTO(comment.Value.Text, comment.Value.Rating + 1);
            await UpdateComment(id, dto);
        }

        public async Task DownvoteComment(int id)
        {
            var comment = await _repo.GetCommentByCommentId(id);

            if (comment.IsNone)
            {
                throw new Exception("The comment could not be found");
            }

            CommentUpdateDTO dto = new CommentUpdateDTO(comment.Value.Text, comment.Value.Rating - 1);
            await UpdateComment(id, dto);
        }

        public async Task<List<Comment>> GetCommentsFromContentId(int contentId)
        {
            var (comments, result) = await _repo.GetCommentsByContentId(contentId);

            if (result == OperationResult.NotFound)
            {
                throw new Exception("The content that the comments belongs to could not be found");
            }

            return comments;
        }

        public async Task<Comment> GetCommentFromCommentId(int commentId)
        {
            var comment = await _repo.GetCommentByCommentId(commentId);

            if (comment.IsNone)
            {
                throw new Exception("The comment could not be found");
            }

            return comment.Value;
        }
    }
}
