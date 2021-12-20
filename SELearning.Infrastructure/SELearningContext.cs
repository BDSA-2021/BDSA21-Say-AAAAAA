namespace SELearning.Infrastructure;

public class SELearningContext : DbContext, ISELearningContext
{
    public DbSet<User.User> Users => Set<User.User>();

    public DbSet<Content.Content> Content => Set<Content.Content>();

    public DbSet<Section.Section> Section => Set<Section.Section>();

    public DbSet<Comment.Comment> Comments => Set<Comment.Comment>();

    public SELearningContext(DbContextOptions<SELearningContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Rules for entity creation for DB
        builder.Entity<User.User>()
            .HasIndex(t => t.Id)
            .IsUnique();

        // Rules for entity creation for DB
        builder.Entity<Comment.Comment>()
            .HasIndex(t => t.Id)
            .IsUnique();

        builder.Entity<Content.Content>()
            .HasIndex(t => t.Id)
            .IsUnique();
    }
}
