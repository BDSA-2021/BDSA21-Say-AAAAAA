namespace SELearning.Infrastructure;

public interface ISELearningContext : IDisposable
{
    DbSet<User.User> Users { get; }
    DbSet<Section.Section> Section { get; }
    DbSet<Content.Content> Content { get; }
    public DbSet<Comment.Comment> Comments { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
