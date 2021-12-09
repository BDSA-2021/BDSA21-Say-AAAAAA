using SELearning.Core.Comment;
namespace SELearning.Infrastructure;
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
        if (content == null)
        {
            return (OperationResult.NotFound, null!);
        }

        Comment comment = new Comment(
            cmt.Text,
            null,
            null,
            content,
            cmt.Author
        );

        _context.Comments.Add(comment);

        await _context.SaveChangesAsync();

        return (OperationResult.Created, ConvertToDetailsDTO(comment));
    }

    public async Task<(OperationResult, CommentDetailsDTO?)> UpdateComment(int Id, CommentUpdateDTO cmt)
    {
        Comment? c = await _context
            .Comments
            .Include(x => x.Author)
            .Include(x => x.Content)
            .FirstOrDefaultAsync(c => c.Id == Id);

        if (c == null)
        {
            return (OperationResult.NotFound, null);
        }

        c.Text = cmt.Text;
        c.Rating = cmt.Rating;

        await _context.SaveChangesAsync();

        return (OperationResult.Updated, ConvertToDetailsDTO(c));
    }

    public async Task<OperationResult> RemoveComment(int Id)
    {
        Comment? comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == Id);

        if (comment == null)
        {
            return (OperationResult.NotFound);
        }

        _context.Comments.Remove(comment);

        await _context.SaveChangesAsync();

        return (OperationResult.Deleted);
    }

    public async Task<Option<CommentDetailsDTO>> GetCommentByCommentId(int commentId)
    {
        var comment = await _context
            .Comments
            .Include(c => c.Author)
            .Include(x => x.Content)
            .FirstOrDefaultAsync(x => x.Id == commentId);
        return comment != null ? ConvertToDetailsDTO(comment) : null;
    }

    public async Task<(IEnumerable<CommentDetailsDTO>?, OperationResult)> GetCommentsByContentId(int contentId)
    {
        Content? content = await _context.Content.FirstOrDefaultAsync(c => c.Id == contentId);
        if (content == null)
        {
            return (null, OperationResult.NotFound);
        }

        IList<CommentDetailsDTO> comments = await _context
            .Comments
            .Include(x => x.Author)
            .Include(x => x.Content)
            .Where(x => x.Content.Id == contentId).Select(x => ConvertToDetailsDTO(x)).ToListAsync();

        return (comments, OperationResult.Succes);
    }

    public async Task<(IEnumerable<CommentDetailsDTO>, OperationResult)> GetCommentsByAuthor(string userId)
    {
        IList<CommentDetailsDTO> commentsByAuthor = await _context
            .Comments
            .Include(x => x.Author)
            .Include(x => x.Content)
            .Where(x => x.Author.Id == userId)
            .Select(x => ConvertToDetailsDTO(x))
            .ToListAsync();

        return (commentsByAuthor, OperationResult.Succes);
    }

    private static CommentDetailsDTO ConvertToDetailsDTO(Comment c) => new(c.Author, c.Text, c.Id, c.Timestamp, c.Rating, c.Content.Id);

}