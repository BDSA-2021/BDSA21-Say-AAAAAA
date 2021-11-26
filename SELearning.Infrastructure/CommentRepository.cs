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

        public Task<(OperationResult, CommentDetailsDTO)> AddComment(CommentCreateDTO cmt)
        {
            Comment c = new Comment
            {
                Author = cmt.Author,
                Text = cmt.Text
            };

            _context.Comments.Add(c);

            _context.SaveChanges();

            CommentDetailsDTO dto = new CommentDetailsDTO(c.Author, c.Text, c.Id, c.Timestamp, c.Rating, c.Content);

            return Task.FromResult((OperationResult.Created, dto));
        }
        //hvis id ikke findes returner notfound, ellers updated
        public Task<OperationResult> UpdateComment(int Id, Comment cmt)
        {
            return Task.FromResult(OperationResult.BadRequest);
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