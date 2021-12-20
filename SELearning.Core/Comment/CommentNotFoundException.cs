using SELearning.Core.Exceptions;

namespace SELearning.Core.Comment;

public class CommentNotFoundException : NotFoundException
{
    public CommentNotFoundException(int id)
        : base("Comment with id could not be found. ID: " + id)
    {
    }
}
