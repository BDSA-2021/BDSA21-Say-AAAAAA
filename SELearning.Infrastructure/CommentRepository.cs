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

        public async Task<(OperationResult, CommentDetailsDTO)> AddComment(CommentDTO cmt)
        {
            Comment c = new Comment
            {
                Author = cmt.Author,
                Text = cmt.Text
            };

            _context.Comments.Add(c);

            _context.SaveChanges();

            CommentDetailsDTO dto = new CommentDetailsDTO(c.Author, c.Text, c.Id, c.Timestamp, c.Rating, c.Content);

            return (OperationResult.Created, dto);
        }
        //hvis id ikke findes returner notfound, ellers updated
        public async Task<(OperationResult,CommentDetailsDTO?)> UpdateComment(int Id, CommentDTO cmt)
        {
            Comment? c = await _context.Comments.FirstOrDefaultAsync(c => c.Id == Id);

            if(c == null)
            {
                return (OperationResult.NotFound, null);
            }

            c.Author = cmt.Author;
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