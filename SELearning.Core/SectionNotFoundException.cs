namespace SELearning.Core
{
    public class SectionNotFoundException : NotFoundException
    {
        public SectionNotFoundException(int id)
            : base("Section with id could not be found. ID: " + id)
        {
        }
    }
}