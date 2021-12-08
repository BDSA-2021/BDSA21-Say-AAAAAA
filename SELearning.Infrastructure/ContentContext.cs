namespace SELearning.Infrastructure;

public class ContentContext : DbContext, IContentContext
{
    public ContentContext(DbContextOptions options) : base(options) { }

    public DbSet<Content> Content => Set<Content>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Rules for entity creation for DB
        builder.Entity<Content>()
            .HasIndex(t => t.Id)
            .IsUnique();
    }
}