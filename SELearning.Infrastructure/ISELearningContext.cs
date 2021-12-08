namespace SELearning.Infrastructure;

public interface ISELearningContext : IDisposable
{
    DbSet<Section> Section { get; }
    DbSet<Content> Content { get; }
    public DbSet<Comment> Comments { get; }
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}