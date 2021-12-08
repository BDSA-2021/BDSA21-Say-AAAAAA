using SELearning.Core.Comment;

namespace SELearning.Infrastructure;

public interface ICommentContext : IDisposable
{
    public DbSet<Content> Content { get; }

    public DbSet<Comment> Comments { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}