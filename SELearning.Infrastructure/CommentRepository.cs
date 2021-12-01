using SELearning.Core.Comment;
namespace SELearning.Infrastructure
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CommentContext _context;
        private readonly ContentContext _contentContext;

        public CommentRepository(CommentContext context, ContentContext contentContext)
        {
            _context = context;
            _contentContext = contentContext;
        }

        //TODO: nedenstående skal tjekke dens contentId og returnere notfound hvis det ikke findes
        public async Task<(OperationResult, CommentDetailsDTO)> AddComment(CommentCreateDTO cmt)
        {
            Content content = await _contentContext.Content.FirstOrDefaultAsync(c => c.Id == cmt.ContentId);
            if (content == null)
            {
                return (OperationResult.NotFound, null!);
            }

            Comment comment = new Comment
            {
                Author = cmt.Author,
                Text = cmt.Text,
                Content = content
            };

            _context.Comments.Add(comment);

            await _context.SaveChangesAsync();

            CommentDetailsDTO dto = new CommentDetailsDTO(comment.Author, comment.Text, comment.Id, comment.Timestamp, comment.Rating, comment.Content);

            return (OperationResult.Created, dto);
        }

        public async Task<(OperationResult, CommentDetailsDTO?)> UpdateComment(int Id, CommentUpdateDTO cmt)
        {
            Comment? c = await _context.Comments.FirstOrDefaultAsync(c => c.Id == Id);

            if (c == null)
            {
                return (OperationResult.NotFound, null);
            }

            c.Text = cmt.Text;

            CommentDetailsDTO dto = new CommentDetailsDTO(c.Author, c.Text, c.Id, c.Timestamp, c.Rating, c.Content);

            await _context.SaveChangesAsync();

            return (OperationResult.Updated, dto);
        }

        public async Task<OperationResult> RemoveComment(int Id)
        {
            Comment? comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == Id);

            if (comment == null)
            {
                return (OperationResult.NotFound);
            }

            _context.Remove(comment);

            await _context.SaveChangesAsync();

            return (OperationResult.Deleted);
        }

        public async Task<(Comment?, OperationResult)> GetCommentByCommentId(int commentId)
        {
            Comment comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == commentId);

            if (comment == null)
            {
                return (null, OperationResult.NotFound);
            }

            return (comment, OperationResult.Succes);
        }

        public async Task<(List<Comment>?, OperationResult)> GetCommentsByContentId(int contentId)
        {
            List<Comment> comments = await _context.Comments.Where(x => x.Content.Id == contentId).ToListAsync();

            if (comments == null)
            {
                return (null, OperationResult.NotFound);
            }

            return (comments, OperationResult.Succes);
        }
    }
}