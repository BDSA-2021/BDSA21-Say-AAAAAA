using System.Linq;
using System.Threading.Tasks;

namespace SELearning.Infrastructure.Tests;
public class CommentRepositoryTests
{
    private readonly CommentRepository _repository;
    private readonly SELearningContext _context;

    private static readonly Section section = new()
    {
        Id = 1,
        Title = "C#",
        Description = "C# tools",
        Content = new List<Content>()
    };

    private static readonly Content content = new()
    {
        Id = 1,
        Author = "Sarah",
        Section = section,
        Title = "Video on Entity Core",
        Description = "Nice",
        VideoLink = "www.hej.dk"
    };

    private readonly IEnumerable<Comment> _comments = new List<Comment>()
        {
            new Comment { Author = "Amalie", Id = 1, Text = "Nice", Content = content },
            new Comment { Author = "Albert", Id = 2, Text = "Cool but boring", Content = content },
            new Comment { Author = "Paolo", Id = 3, Text = "This is a great video", Content = content },
            new Comment { Author = "Rasmus", Id = 4, Text = "Very inappropriate", Content = content }
        };

    public CommentRepositoryTests()
    {
        //setting up the comment connection
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<SELearningContext>();
        builder.UseSqlite(connection);
        _context = new SELearningContext(builder.Options);
        _context.Database.EnsureCreated();

        _repository = new CommentRepository(_context);

        section.Content!.Add(content);

        _context.Comments.AddRange(
            _comments
        );

        _context.SaveChanges();
    }

    [Fact]
    public async Task AddComment_creates_new_comment_with_generated_id()
    {
        CommentCreateDTO comment = new("Harleen", "Nice content", 1);

        var created = await _repository.AddComment(comment);

        Assert.Equal(5, created.Item2.Id);
        Assert.Equal("Harleen", created.Item2.Author);
        Assert.Equal("Nice content", created.Item2.Text);
        Assert.Equal(OperationResult.Created, created.Item1);
    }

    [Fact]
    public async Task AddComment__given_non_existing_ContentId_returns_NotFound()
    {
        CommentCreateDTO comment = new("Harleen", "Nice content", 2);

        var created = await _repository.AddComment(comment);

        Assert.Equal(OperationResult.NotFound, created.Item1);
    }

    [Fact]
    public async Task UpdateComment_given_non_existing_id_returns_NotFound()
    {
        CommentUpdateDTO dto = new("Really like this content", 0);

        var updated = await _repository.UpdateComment(42, dto);

        Assert.Equal(OperationResult.NotFound, updated.Item1);
    }

    [Fact]
    public async Task UpdateComment_updates_existing_comment()
    {
        CommentUpdateDTO dto = new("Nice but also confusing", 1);

        var (result, updated) = await _repository.UpdateComment(1, dto);

        Assert.Equal(1, updated!.Id);
        Assert.Equal("Amalie", updated.Author);
        Assert.Equal("Nice but also confusing", updated.Text);
        Assert.Equal(1, updated.Rating);

        Assert.Equal(OperationResult.Updated, result);
    }

    [Fact]
    public async Task RemoveComment_given_not_existing_id_returns_NotFound()
    {
        var removed = await _repository.RemoveComment(68);

        Assert.Equal(OperationResult.NotFound, removed);
    }

    [Fact]
    public async Task RemoveComment_given_existing_id_removes_comment()
    {
        var removed = await _repository.RemoveComment(2);

        Assert.Equal(OperationResult.Deleted, removed);

        var tryRead = await _repository.GetCommentByCommentId(2);
        Assert.True(tryRead.IsNone);
    }

    [Fact]
    public async Task GetCommentByCommentId_given_existing_id_returns_comment()
    {
        var read = (await _repository.GetCommentByCommentId(3)).Value;

        Assert.Equal("Paolo", read.Author);
        Assert.Equal("This is a great video", read.Text);
    }

    [Fact]
    public async Task GetCommentByCommentId_given_not_existing_id_returns_null()
    {
        var read = await _repository.GetCommentByCommentId(90);

        Assert.True(read.IsNone);
    }

    [Fact]
    public async Task GetCommentsByContentId_given_existing_id_returns_comments()
    {
        var read = await _repository.GetCommentsByContentId(1);

        Assert.Equal(OperationResult.Succes, read.Item2);
        Assert.Equal(_comments.Select(x => new CommentDetailsDTO(x.Author, x.Text, x.Id, x.Timestamp, x.Rating, x.Content.Id)), read.Item1);
    }

    [Fact]
    public async Task GetCommentsByContentId_given_not_existing_id_returns_NotFound()
    {
        var read = await _repository.GetCommentsByContentId(90);

        Assert.Equal(OperationResult.NotFound, read.Item2);
    }

    [Fact]
    public async Task GetCommentsByAuthor_GivenComments_ReturnsByAuthor()
    {
        await _context.Comments.AddRangeAsync(new[]
        {
            new Comment { Author = "Sankt Nikolaus", Id = 98, Content = content },
            new Comment { Author = "Julemanden", Id = 97, Content = content },
            new Comment { Author = "Santa Claus", Id = 96, Content = content },
            new Comment { Author = "Julemanden", Id = 95, Content = content },
        });
        await _context.SaveChangesAsync();

        var (comments, opResult) = await _repository.GetCommentsByAuthor("Julemanden");

        Assert.Equal(OperationResult.Succes, opResult);

        var expectedIds = new[] { 95, 97 };
        var actualIds = comments.OrderBy(c => c.Id).Select(c => (int)c.Id!).ToList();
        Assert.Equal(expectedIds, actualIds);
    }
}
