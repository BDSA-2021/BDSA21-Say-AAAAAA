using SELearning.Core.Comment;
namespace SELearning.Infrastructure
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CommentContext _context;

        public CommentRepository(CommentContext context)
        {
            _context = context;
        }

        public async Task<(OperationResult, CommentDetailsDTO)> AddComment(CommentCreateDTO cmt)
        {
            Content? content = await _context.Content.FirstOrDefaultAsync(c => c.Id == cmt.ContentId);
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
            c.Rating = cmt.Rating;

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

        public async Task<Option<Comment>> GetCommentByCommentId(int commentId)
        {
            return await _context.Comments.FirstOrDefaultAsync(x => x.Id == commentId);
        }

        public async Task<(List<Comment>?, OperationResult)> GetCommentsByContentId(int contentId)
        {
            Content? content = await _context.Content.FirstOrDefaultAsync(c => c.Id == contentId);
            if (content == null)
            {
                return (null, OperationResult.NotFound);
            }

            List<Comment> comments = await _context.Comments.Where(x => x.Content.Id == contentId).ToListAsync();

            return (comments, OperationResult.Succes);
        }
    }
}