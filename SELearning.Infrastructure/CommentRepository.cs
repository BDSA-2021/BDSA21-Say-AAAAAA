using SELearning.Core.Comment;
namespace SELearning.Infrastructure
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CommentContext _context;
        private readonly ContentContext _contentContext;

        public CommentRepository(CommentContext context)
        {
            _context = context;
        }

        //TODO: nedenstående skal tjekke dens contentId og returnere notfound hvis det ikke findes
        public async Task<(OperationResult, CommentDetailsDTO)> AddComment(CommentCreateDTO cmt)
        {
            Content content = await _contentContext.Content.FirstOrDefaultAsync(c => c.Id == cmt.ContentId);
            if(content == null)
            {
                return (OperationResult.NotFound, null);
            }

            Comment c = new Comment
            {
                Author = cmt.Author,
                Text = cmt.Text,
                Content = content
            };

            _context.Comments.Add(c);

            _context.SaveChanges();

            CommentDetailsDTO dto = new CommentDetailsDTO(c.Author, c.Text, c.Id, c.Timestamp, c.Rating, c.Content);

            return (OperationResult.Created, dto);
        }
        
        public async Task<(OperationResult,CommentDetailsDTO?)> UpdateComment(int Id, CommentUpdateDTO cmt)
        {
            Comment? c = await _context.Comments.FirstOrDefaultAsync(c => c.Id == Id);

            if(c == null)
            {
                return (OperationResult.NotFound, null);
            }

            c.Text = cmt.Text;

            CommentDetailsDTO dto = new CommentDetailsDTO(c.Author, c.Text, c.Id, c.Timestamp, c.Rating, c.Content);

            return (OperationResult.Updated, dto);
        }

        //hvis id ikke findes returner notfound, ellers deleted

        public Task<OperationResult> RemoveComment(int Id)
        {
            return Task.FromResult(OperationResult.BadRequest);
        }

        public Task<List<Comment>> GetCommentsByContentId(int contentId)
        {
            //_context.Comments.Where(x => x.Content.Id == contentId);
            return Task.FromResult(new List<Comment>());
        }
    }
}