namespace SELearning.Core
{
    public class ContentNotFoundException : NotFoundException
    {
        public ContentNotFoundException(int id)
            : base("Content with id could not be found. ID: " + id)
        {
        }
    }
}