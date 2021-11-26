namespace SELearning.Infrastructure;

public interface IContentContext : IDisposable
{
    DbSet<Content> Content { get; }
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}