using SELearning.Core.Section;
using SELearning.Core.User;

namespace SELearning.Infrastructure;

public class SELearningContext : DbContext, ISELearningContext
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Content> Content => Set<Content>();

    public DbSet<Section> Section => Set<Section>();

    public DbSet<Comment> Comments => Set<Comment>();

    public SELearningContext(DbContextOptions<SELearningContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Rules for entity creation for DB
        builder.Entity<User>()
            .HasIndex(t => t.Id)
            .IsUnique();

        // Rules for entity creation for DB
        builder.Entity<Comment>()
            .HasIndex(t => t.Id)
            .IsUnique();

        builder.Entity<Content>()
            .HasIndex(t => t.Id)
            .IsUnique();

    }
}