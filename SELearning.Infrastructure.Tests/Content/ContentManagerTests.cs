using System;
using System.Threading.Tasks;
using SELearning.Core.User;

namespace SELearning.Infrastructure.Tests;

public class ContentManagerTests : IDisposable
{
    private readonly SELearningContext _context;
    private readonly ContentRepository _repository;
    private readonly ContentManager _manager;
    private readonly Section.Section _section;

    private readonly User.User _user;
    private bool disposedValue;

    public ContentManagerTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<SELearningContext>();
        builder.UseSqlite(connection);

        var context = new SELearningContext(builder.Options);
        context.Database.EnsureCreated();

        _user = new User { Id = "toucan", Name = "Næbdyr" };

        var content1 = new Content("title", "description", "link", 3, _user, _section);
        var content2 = new Content("title", "description", "link", 3, _user, _section);
        var content3 = new Content("title", "description", "link", 3, _user, _section);
        var content4 = new Content("title", "description", "link", 3, _user, _section);
        content1.Id = 1;
        content1.Id = 2;
        content1.Id = 3;
        content1.Id = 4;

        _section = new Section { Id = 1, Title = "python", Description = "description" };
        _section.Content = new List<Content>
        {
            content1,
            content2,
            content3,
            content4
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
            Author = _user,
            Title = "title",
            Description = "description",
            VideoLink = "video link",
        };

        await _manager.AddContent(content);

        var contentWithID = (await _repository.GetContent(5)).Value;

        Assert.Equal(_section, contentWithID.Section);
        Assert.Equal(_user, contentWithID.Author);
        Assert.Equal("title", contentWithID.Title);
        Assert.Equal("description", contentWithID.Description);
        Assert.Equal("video link", contentWithID.VideoLink);
        Assert.Equal(0, contentWithID.Rating);
    }

    [Fact]
    public async Task GetContent_by_id_returns_content()
    {
        var content = await _manager.GetContent(1);

        Assert.Equal(1, content.Id);
        Assert.Equal(_section, content.Section);
        Assert.Equal(_user, content.Author);
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

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~CharacterRepositoryTests()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
