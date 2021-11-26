namespace SELearning.Infrastructure;

public class ContentContext : DbContext, IContentContext
{
    public ContentContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Content> Content => throw new NotImplementedException();
}