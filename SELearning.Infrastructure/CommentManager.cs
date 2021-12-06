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
                throw new ContentNotFoundException(dto.ContentId);
            }
        }

        public async Task UpdateComment(int id, CommentUpdateDTO dto)
        {
            if ((await _repo.UpdateComment(id, dto)).Item1 == OperationResult.NotFound)
            {
                throw new CommentNotFoundException(id);
            }
        }

        public async Task RemoveComment(int id)
        {
            if (await _repo.RemoveComment(id) == OperationResult.NotFound)
            {
                throw new CommentNotFoundException(id);
            }
        }

        public async Task UpvoteComment(int id)
        {
            var comment = await _repo.GetCommentByCommentId(id);

            if (comment.IsNone)
            {
                throw new CommentNotFoundException(id);
            }

            CommentUpdateDTO dto = new CommentUpdateDTO(comment.Value.Text, comment.Value.Rating + 1);
            await UpdateComment(id, dto);
        }

        public async Task DownvoteComment(int id)
        {
            var comment = await _repo.GetCommentByCommentId(id);

            if (comment.IsNone)
            {
                throw new CommentNotFoundException(id);
            }

            CommentUpdateDTO dto = new CommentUpdateDTO(comment.Value.Text, comment.Value.Rating - 1);
            await UpdateComment(id, dto);
        }

        public async Task<List<Comment>> GetCommentsFromContentId(int contentId)
        {
            var (comments, result) = await _repo.GetCommentsByContentId(contentId);

            if (result == OperationResult.NotFound)
            {
                throw new ContentNotFoundException(contentId);
            }

            return comments;
        }

        public async Task<Comment> GetCommentFromCommentId(int id)
        {
            var comment = await _repo.GetCommentByCommentId(id);

            if (comment.IsNone)
            {
                throw new CommentNotFoundException(id);
            }

            return comment.Value;
        }
    }
}
