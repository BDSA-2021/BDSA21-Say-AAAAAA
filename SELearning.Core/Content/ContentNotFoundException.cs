using SELearning.Core.Exceptions;

namespace SELearning.Core.Content;

public class ContentNotFoundException : NotFoundException
{
    public ContentNotFoundException(int id)
        : base("Content with id could not be found. ID: " + id)
    {
    }
}
