using System.Threading.Tasks;
using System.Linq;
using SELearning.Core.User;

namespace SELearning.Infrastructure.Tests;

public class ContentRepositoryTests
{
    private readonly SELearningContext _context;
    private readonly ContentRepository _repository;
    private readonly Section.Section _section;

    private static readonly User.User AuthorUser = new()
    {
        Id = "author",
        Name = "author"
    };

    public ContentRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<SELearningContext>();
        builder.UseSqlite(connection);
        _context = new SELearningContext(builder.Options);
        _context.Database.EnsureCreated();

        _section = new Section.Section { Id = 1, Title = "python", Description = "description" };

        var contentList = new List<Content.Content>
        {
            new("title", "description", "VideoLink", 3, AuthorUser, _section),
            new("title", "description", "VideoLink", 3, AuthorUser, _section),
            new("title", "description", "VideoLink", 3, AuthorUser, _section),
            new("title", "description", "VideoLink", 3, AuthorUser, _section)
        };

        _context.Content.AddRange(contentList);
        _context.Section.Add(_section);
        _context.Users.Add(AuthorUser);

        _context.SaveChanges();

        _repository = new ContentRepository(_context);
    }

    [Fact]
    public async Task CreateContentAsync_creates_new_content_with_generated_id()
    {
        var content = new ContentCreateDto
        {
            Title = "title",
            Description = "description",
            VideoLink = "video link",
            SectionId = _section.Id,
            Author = AuthorUser.ToUserDTO()
        };

        var created = (await _repository.AddContent(content)).Item2;

        Assert.Equal(_section.ToSectionDTO(), created.Section);
        Assert.Equal("author", created.Author.Id);
        Assert.Equal("title", created.Title);
        Assert.Equal("description", created.Description);
        Assert.Equal("video link", created.VideoLink);
        Assert.Equal(0, created.Rating);
    }

    [Fact]
    public async Task CreateContentAsync_GivenContent_ReturnsCreated()
    {
        var content = new ContentCreateDto
        {
            Title = "title",
            Description = "description",
            VideoLink = "video link",
            SectionId = _section.Id,
            Author = AuthorUser.ToUserDTO()
        };

        var (status, created) = await _repository.AddContent(content);

        var contentDto = new ContentDTO
        {
            Id = 5,
            Title = "title",
            Description = "description",
            VideoLink = "video link",
            Rating = 0,
            Section = _section.ToSectionDTO(),
            Author = AuthorUser.ToUserDTO()
        };

        Assert.Equal(contentDto, created);
        Assert.Equal(OperationResult.Created, status);
    }

    [Fact]
    public async Task ReadContentAsync_given_non_existing_id_returns_None()
    {
        var option = await _repository.GetContent(42);

        Assert.True(option.IsNone);
    }

    [Fact]
    public async Task UpdateContentAsync_given_non_existing_id_returns_NotFound()
    {
        var content = new ContentUpdateDTO
        {
            Title = "title",
            Description = "description",
            VideoLink = "video link",
            Rating = 0
        };

        var reponse = await _repository.UpdateContent(42, content);

        Assert.Equal(OperationResult.NotFound, reponse);
    }

    [Fact]
    public async Task ReadContentAsync_given_existing_id_returns_Content()
    {
        var option = await _repository.GetContent(1);

        var content = new ContentDTO
        {
            Id = 1,
            Section = _section.ToSectionDTO(),
            Author = new UserDTO("author", "author"),
            Title = "title",
            Description = "description",
            VideoLink = "VideoLink",
            Rating = 3
        };

        Assert.Equal(content, option.Value);
    }


    [Fact]
    public async Task ReadContentAsync_returns_all_content()
    {
        var allContent = await _repository.GetContent();

        Assert.All(allContent,
            content =>
            {
                Assert.NotNull(content);
                Assert.Equal("author", content.Author.Id);
                Assert.Equal("title", content.Title);
                Assert.Equal("description", content.Description);
                Assert.Equal("VideoLink", content.VideoLink);
                Assert.Equal(3, content.Rating);
            }
        );
    }


    [Fact]
    public async Task UpdateContentAsync_updates_existing_content()
    {
        var content = new ContentUpdateDTO
        { Title = "title", Description = "description", VideoLink = "video link", Rating = 3 };

        var updated = await _repository.UpdateContent(1, content);

        Assert.Equal(OperationResult.Updated, updated);
    }

    [Fact]
    public async Task UpdateContentAsync_given_non_existing_Content_returns_NotFound()
    {
        var content = new ContentUpdateDTO
        { Title = "title", Description = "description", VideoLink = "video link", Rating = 3 };

        var response = await _repository.UpdateContent(42, content);

        Assert.Equal(OperationResult.NotFound, response);
    }


    [Fact]
    public async Task UpdateContentAsync_updates_and_returns_Updated()
    {
        var contentDto = new ContentUpdateDTO
        { Title = "new title", Description = "description", VideoLink = "VideoLink", Rating = 3 };

        var response = await _repository.UpdateContent(1, contentDto);

        await _context.Content.FirstAsync(c => c.Title == "new title");

        Assert.Equal(OperationResult.Updated, response);
    }

    [Fact]
    public async Task DeleteContentAsync_given_non_existing_Id_returns_NotFound()
    {
        var response = await _repository.DeleteContent(42);

        Assert.Equal(OperationResult.NotFound, response);
    }

    [Fact]
    public async Task DeleteContentAsync_deletes_and_returns_Deleted()
    {
        var response = await _repository.DeleteContent(2);

        var entity = await _context.Content.FindAsync(2);

        Assert.Equal(OperationResult.Deleted, response);
        Assert.Null(entity);
    }

    [Fact]
    public async Task GetContentByAuthor_GivenContent_ReturnsContentFromSpecifiedAuthor()
    {
        await _context.Content.AddRangeAsync(
            new Content.Content("title", "description", "VideoLink", 3, AuthorUser, _section), new Content.Content(
                "title", "description", "VideoLink", 3, new User.User { Id = "homer", Name = "homer" },
                _section), new Content.Content("title", "description", "VideoLink", 3, AuthorUser, _section));
        await _context.SaveChangesAsync();

        var contents = await _repository.GetContentByAuthor("homer");

        var expectedIds = new[] { 7 };
        var actualIds = contents.OrderBy(c => c.Id).Select(c => c.Id).ToList();
        Assert.Equal(expectedIds, actualIds);
    }
}
