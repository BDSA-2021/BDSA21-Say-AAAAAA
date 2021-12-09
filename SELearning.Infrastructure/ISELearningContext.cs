namespace SELearning.Infrastructure;

public interface ISELearningContext : IDisposable
{
    DbSet<User> Users { get; }
    DbSet<Section> Section { get; }
    DbSet<Content> Content { get; }
    public DbSet<Comment> Comments { get; }
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}