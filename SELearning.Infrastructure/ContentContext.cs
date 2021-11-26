namespace SELearning.Infrastructure;

public class ContentContext : DbContext, IContentContext
{
    public DbSet<Content> Content => Set<Content>();

    public ContentContext(DbContextOptions<ContentContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}