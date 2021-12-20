using System.Linq;
using System.Threading.Tasks;
using SELearning.Core.User;

namespace SELearning.Infrastructure.Tests;

public class CommentRepositoryTests
{
    private readonly CommentRepository _repository;

    private static readonly Section.Section Section = new()
    {
        Id = 1,
        Title = "C#",
        Description = "C# tools",
        Content = new List<Content.Content>()
    };

    private static readonly Content.Content Content = new(
        "Video on Entity Core",
        "Nice",
        "www.hej.dk",
        null,
        new User.User
        {
            Id = "Sarah",
            Name = "Sarah"
        },
        Section
    );

    private readonly IEnumerable<Comment.Comment> _comments;
    private readonly User.User _userAmalie = new() { Id = "Amalie", Name = "Amalie" };

    public CommentRepositoryTests()
    {
        //setting up the comment connection
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<SELearningContext>();
        builder.UseSqlite(connection);
        var context = new SELearningContext(builder.Options);
        context.Database.EnsureCreated();

        _comments = new List<Comment.Comment>
        {
            new("Nice", null, null, Content, _userAmalie),
            new("Cool but boring", null, null, Content, new User.User {Id = "Albert", Name = "Albert"}),
            new("This is a great video", null, null, Content,
                new User.User {Id = "Paolo", Name = "Paolo"}),
            new("Very inappropriate", null, null, Content,
                new User.User {Id = "Rasmus", Name = "Rasmus"}),
            new("Nicer", null, null, Content, _userAmalie)
        };


        _repository = new CommentRepository(context);

        Section.Content!.Add(Content);

        context.Comments.AddRange(
            _comments
        );

        context.SaveChanges();
    }

    [Fact]
    public async Task AddComment_creates_new_comment_with_generated_id()
    {
        CommentCreateDTO comment = new(_userAmalie.ToUserDTO(), "Nice content", 1);

        var (operationResult, commentDetailsDto) = await _repository.AddComment(comment);

        Assert.Equal(6, commentDetailsDto.Id);
        Assert.Equal("Amalie", commentDetailsDto.Author.Name);
        Assert.Equal("Nice content", commentDetailsDto.Text);
        Assert.Equal(OperationResult.Created, operationResult);
    }

    [Fact]
    public async Task AddComment__given_non_existing_ContentId_returns_NotFound()
    {
        CommentCreateDTO comment = new(new UserDTO("Harleen", "Harleen"), "Nice content", 2);

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
        Assert.Equal("Amalie", updated.Author.Name);
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

        Assert.Equal("Paolo", read.Author.Name);
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
        var (commentDetailDtos, operationResult) = await _repository.GetCommentsByContentId(1);

        Assert.Equal(OperationResult.Succes, operationResult);
        Assert.Equal(
            _comments.Select(x =>
                new CommentDetailsDTO(x.Author.ToUserDTO(), x.Text, x.Id, x.Timestamp, x.Rating, x.Content.Id)),
            commentDetailDtos);
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
        var (comments, opResult) = await _repository.GetCommentsByAuthor("Amalie");

        Assert.Equal(OperationResult.Succes, opResult);

        var expectedIds = new[] { 1, 5 };
        var actualIds = comments.OrderBy(c => c.Id).Select(c => c.Id).ToList();
        Assert.Equal(expectedIds, actualIds);
    }
}
