using SELearning.Core.Exceptions;

namespace SELearning.Core.Section;

public class SectionNotFoundException : NotFoundException
{
    public SectionNotFoundException(int id)
        : base("Section with id could not be found. ID: " + id)
    {
    }
}
