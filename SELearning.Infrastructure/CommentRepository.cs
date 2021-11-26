namespace SELearning.Infrastructure
{
    public class CommentRepository: ICommentRepository
    {
        private readonly CommentContext dbContext;

        public CommentRepository(CommentContext context)
        {
            dbContext = context;
        }
        //returner created
        public async Task<OperationResult> AddComment(Comment cmt)
        {
            return OperationResult.BadRequest;
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
            return new List<Comment>();
        }
    }
}