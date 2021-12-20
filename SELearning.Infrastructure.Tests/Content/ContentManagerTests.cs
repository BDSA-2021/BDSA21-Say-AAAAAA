using System.Threading.Tasks;

namespace SELearning.Infrastructure.Tests;

public class ContentManagerTests
{
    private readonly SELearningContext _context;
    private readonly ContentRepository _repository;
    private readonly ContentManager _manager;
    private readonly Section.Section _section;

    private readonly User.User _user;

    public ContentManagerTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<SELearningContext>();
        builder.UseSqlite(connection);

        var context = new SELearningContext(builder.Options);
        context.Database.EnsureCreated();

        _user = new User.User { Id = "toucan", Name = "Næbdyr" };

        var content1 = new Infrastructure.Content.Content("title", "description", "link", 3, _user, _section);
        var content2 = new Infrastructure.Content.Content("title", "description", "link", 3, _user, _section);
        var content3 = new Infrastructure.Content.Content("title", "description", "link", 3, _user, _section);
        var content4 = new Infrastructure.Content.Content("title", "description", "link", 3, _user, _section);
        content1.Id = 1;
        content1.Id = 2;
        content1.Id = 3;
        content1.Id = 4;

        _section = new Section.Section
        {
            Id = 1,
            Title = "python",
            Description = "description",
            Content = new List<Infrastructure.Content.Content>
            {
                content1,
                content2,
                content3,
                content4
            }
        };

        context.Content.AddRange(content1, content2, content3, content4);
        context.Section.Add(_section);
        context.SaveChanges();

        _context = context;
        _repository = new ContentRepository(_context);
        _manager = new ContentManager(_repository);
    }

    [Fact]
    public async Task IncreaseContentRating_increase_existing_Content_rating_returns_updated()
    {
        await _manager.IncreaseContentRating(1);

        var entity = await _context.Content.FirstAsync(c => c.Id == 1);

        Assert.Equal(4, entity.Rating);
    }

    [Fact]
    public async Task DecreaseContentRating_decrease_existing_Content_rating_returns_updated()
    {
        await _manager.DecreaseContentRating(1);

        var entity = await _context.Content.FirstAsync(c => c.Id == 1);

        Assert.Equal(2, entity.Rating);
    }

    [Fact]
    public async Task DeleteContent_do_not_throw_error_and_deletes_section()
    {
        await _manager.DeleteContent(1);

        var entity = await _context.Content.FindAsync(1);

        Assert.Null(entity);
    }


    [Fact]
    public async Task CreateContentAsync_creates_new_content_with_generated_id()
    {
        var content = new ContentCreateDto
        {
            SectionId = _section.Id,
            Author = _user.ToUserDTO(),
            Title = "title",
            Description = "description",
            VideoLink = "video link"
        };

        await _manager.AddContent(content);

        var contentWithId = (await _repository.GetContent(5)).Value;

        Assert.Equal(_section.ToSectionDTO(), contentWithId.Section);
        Assert.Equal(_user.ToUserDTO(), contentWithId.Author);
        Assert.Equal("title", contentWithId.Title);
        Assert.Equal("description", contentWithId.Description);
        Assert.Equal("video link", contentWithId.VideoLink);
        Assert.Equal(0, contentWithId.Rating);
    }

    [Fact]
    public async Task GetContent_by_id_returns_content()
    {
        var content = await _manager.GetContent(1);

        Assert.Equal(1, content.Id);
        Assert.Equal(_section.ToSectionDTO(), content.Section);
        Assert.Equal(_user.ToUserDTO(), content.Author);
        Assert.Equal("title", content.Title);
        Assert.Equal("description", content.Description);
        Assert.Equal("link", content.VideoLink);
        Assert.Equal(3, content.Rating);
    }

    [Fact]
    public async Task GetContent_returns_all_Content()
    {
        var allContent = await _manager.GetContent();

        Assert.Collection(allContent,
            content => Assert.Equal(1, content.Id),
            content => Assert.Equal(2, content.Id),
            content => Assert.Equal(3, content.Id),
            content => Assert.Equal(4, content.Id)
        );
    }
}
