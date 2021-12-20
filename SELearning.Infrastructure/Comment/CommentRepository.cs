namespace SELearning.Infrastructure.Comment;

public class CommentRepository : ICommentRepository
{
    private readonly ISELearningContext _context;

    public CommentRepository(ISELearningContext context)
    {
        _context = context;
    }

    public async Task<(OperationResult, CommentDetailsDTO)> AddComment(CommentCreateDTO cmt)
    {
        var content = await _context.Content.FirstOrDefaultAsync(c => c.Id == cmt.ContentId);
        var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == cmt.Author.Id);

        if (content == null || user == null)
        {
            return (OperationResult.NotFound, null!);
        }

        var comment = new Comment(
            cmt.Text,
            null,
            null,
            content,
            user
        );

        _context.Comments.Add(comment);

        await _context.SaveChangesAsync();

        return (OperationResult.Created, comment.ToDetailsDTO());
    }

    public async Task<(OperationResult, CommentDetailsDTO?)> UpdateComment(int id, CommentUpdateDTO cmt)
    {
        var c = await _context
            .Comments
            .Include(x => x.Author)
            .Include(x => x.Content)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (c == null)
        {
            return (OperationResult.NotFound, null);
        }


        (c.Text, c.Rating) = cmt;

        await _context.SaveChangesAsync();

        return (OperationResult.Updated, c.ToDetailsDTO());
    }

    public async Task<OperationResult> RemoveComment(int id)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
        {
            return OperationResult.NotFound;
        }

        _context.Comments.Remove(comment);

        await _context.SaveChangesAsync();

        return OperationResult.Deleted;
    }

    public async Task<Option<CommentDetailsDTO>> GetCommentByCommentId(int commentId)
    {
        var comment = await _context
            .Comments
            .Include(c => c.Author)
            .Include(x => x.Content)
            .FirstOrDefaultAsync(x => x.Id == commentId);
        return comment?.ToDetailsDTO();
    }

    public async Task<(IEnumerable<CommentDetailsDTO>?, OperationResult)> GetCommentsByContentId(int contentId)
    {
        var content = await _context.Content.FirstOrDefaultAsync(c => c.Id == contentId);
        if (content == null)
        {
            return (null, OperationResult.NotFound);
        }

        IList<CommentDetailsDTO> comments = await _context
            .Comments
            .Include(x => x.Author)
            .Include(x => x.Content)
            .Where(x => x.Content.Id == contentId)
            .Select(x => x.ToDetailsDTO()).ToListAsync();

        return (comments, OperationResult.Succes);
    }

    public async Task<(IEnumerable<CommentDetailsDTO>, OperationResult)> GetCommentsByAuthor(string userId)
    {
        IList<CommentDetailsDTO> commentsByAuthor = await _context
            .Comments
            .Include(x => x.Author)
            .Include(x => x.Content)
            .Where(x => x.Author.Id == userId)
            .Select(x => x.ToDetailsDTO())
            .ToListAsync();

        return (commentsByAuthor, OperationResult.Succes);
    }
}
