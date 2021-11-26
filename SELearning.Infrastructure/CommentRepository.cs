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
        //returner created
        public async Task<(OperationResult,CommentDetailsDTO)> AddComment(CommentCreateDTO cmt)
        {
            // omdan DTO til comment

            // Tilføj Comment instans yil context.dbset

            // kald savechanges

            //comment har fået et id

            return (OperationResult.BadRequest,new CommentDetailsDTO("","",1,DateTime.Now,1,1));
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