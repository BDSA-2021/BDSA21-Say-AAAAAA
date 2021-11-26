using SELearning.Core.Comment;
namespace SELearning.Infrastructure
{
    public class CommentRepository: ICommentRepository
    {
        private readonly CommentContext _context;

        public CommentRepository(CommentContext context)
        {
            _context = context;
        }
        
        public async Task<(OperationResult,CommentDetailsDTO)> AddComment(CommentCreateDTO cmt)
        {   
            Comment c = new Comment
            {
                Author = cmt.Author,
                Text = cmt.Text
            };

            _context.Comments.Add(c);

            _context.SaveChanges();

            CommentDetailsDTO dto = new CommentDetailsDTO(c.Author,c.Text,c.Id,c.Timestamp,c.Rating,c.Content);

            return (OperationResult.Created,dto);
        }
        //hvis id ikke findes returner notfound, ellers updated
        public async Task<OperationResult> UpdateComment(int Id, Comment cmt)
        {
            return OperationResult.BadRequest;
        }

        //hvis id ikke findes returner notfound, ellers deleted

        public async Task<OperationResult> RemoveComment(int Id)
        {
            return OperationResult.BadRequest;
        }

        public async Task<List<Comment>> GetCommentsByContentId(int contentId)
        {
            //_context.Comments.Where(x => x.Content.Id == contentId);
            return new List<Comment>();
        }
    }
}