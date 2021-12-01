using SELearning.Core.Comment;
namespace SELearning.Infrastructure
{

    public class CommentContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Content> Content { get; set; }

        public CommentContext(DbContextOptions<CommentContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Rules for entity creation for DB
            builder.Entity<Comment>()
                .HasIndex(t => t.Id)
                .IsUnique();

            builder.Entity<Content>()
                .HasIndex(t => t.Id)
                .IsUnique();
        }
    }
}