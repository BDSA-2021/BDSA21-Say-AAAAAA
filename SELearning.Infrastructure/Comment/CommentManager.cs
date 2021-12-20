namespace SELearning.Infrastructure.Comment;

public class CommentManager : ICommentService
{
    private readonly ICommentRepository _repo;

    public CommentManager(ICommentRepository repo)
    {
        _repo = repo;
    }

    public async Task<CommentDetailsDTO> PostComment(CommentCreateDTO dto)
    {
        var (result, comment) = await _repo.AddComment(dto);

        if (result == OperationResult.NotFound)
        {
            throw new ContentNotFoundException(dto.ContentId);
        }

        return comment;
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

        var dto = new CommentUpdateDTO(comment.Value.Text, comment.Value.Rating + 1);
        await UpdateComment(id, dto);
    }

    public async Task DownvoteComment(int id)
    {
        var comment = await _repo.GetCommentByCommentId(id);

        if (comment.IsNone)
        {
            throw new CommentNotFoundException(id);
        }

        var dto = new CommentUpdateDTO(comment.Value.Text, comment.Value.Rating - 1);
        await UpdateComment(id, dto);
    }

    public async Task<IEnumerable<CommentDetailsDTO>> GetCommentsFromContentId(int contentId)
    {
        var (comments, result) = await _repo.GetCommentsByContentId(contentId);

        if (result == OperationResult.NotFound || comments == null)
        {
            throw new ContentNotFoundException(contentId);
        }

        return comments;
    }

    public async Task<CommentDetailsDTO> GetCommentFromCommentId(int id)
    {
        var comment = await _repo.GetCommentByCommentId(id);

        if (comment.IsNone)
        {
            throw new CommentNotFoundException(id);
        }

        return comment.Value;
    }

    public async Task<IEnumerable<CommentDetailsDTO>> GetCommentsByAuthor(string userId)
    {
        var result = await _repo.GetCommentsByAuthor(userId);

        return result.Item1;
    }
}
