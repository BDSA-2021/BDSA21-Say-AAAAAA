using SELearning.Core.Content;
using System;
using System.Threading.Tasks;

namespace SELearning.Infrastructure.Tests;

public class ContentManagerTests : IDisposable
{
    private readonly SELearningContext _context;
    private readonly ContentRepository _repository;
    private readonly ContentManager _manager;
    private readonly Section _section;
    private bool disposedValue;

    public ContentManagerTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<SELearningContext>();
        builder.UseSqlite(connection);

        var context = new SELearningContext(builder.Options);
        context.Database.EnsureCreated();

        var content1 = new Content { Id = 1, Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 };
        var content2 = new Content { Id = 2, Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 };
        var content3 = new Content { Id = 3, Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 };
        var content4 = new Content { Id = 4, Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 };

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
    public async Task DeleteSection_do_not_throw_error_and_deletes_section()
    {
        await _manager.DeleteSection(1);

        var entity = await _context.Section.FindAsync(1);

        Assert.Null(entity);
    }

    [Fact]
    public async Task DeleteContent_do_not_throw_error_and_deletes_section()
    {
        await _manager.DeleteContent(1);

        var entity = await _context.Content.FindAsync(1);

        Assert.Null(entity);
    }

    [Fact]
    public async Task CreateSectionAsync_creates_new_content_with_generated_id()
    {
        var contentList = new List<Content>();
        var section = new SectionCreateDto { Title = "title", Description = "description" };

        await _manager.AddSection(section);

        var option = await _repository.GetSection(2);

        Assert.NotNull(option.Value.Id);
        Assert.Equal("title", option.Value.Title);
        Assert.Equal("description", option.Value.Description);
        Assert.Equal(contentList, option.Value.Content);
    }

    [Fact]
    public async Task CreateContentAsync_creates_new_content_with_generated_id()
    {
        var content = new ContentCreateDto
        {
            Section = _section,
            Author = "author",
            Title = "title",
            Description = "description",
            VideoLink = "video link",
            Rating = 3,
        };

        await _manager.AddContent(content);

        var contentWithID = (await _repository.GetContent(5)).Value;

        Assert.NotNull(contentWithID.Id);
        Assert.Equal(_section, contentWithID.Section);
        Assert.Equal("author", (IEnumerable<char>)contentWithID.Author);
        Assert.Equal("title", contentWithID.Title);
        Assert.Equal("description", contentWithID.Description);
        Assert.Equal("video link", contentWithID.VideoLink);
        Assert.Equal(3, contentWithID.Rating);
    }

    [Fact]
    public async Task GetContent_by_id_returns_content()
    {
        var content = await _manager.GetContent(1);

        Assert.Equal(1, content.Id);
        Assert.Equal(_section, content.Section);
        Assert.Equal("author", (IEnumerable<char>)content.Author);
        Assert.Equal("title", content.Title);
        Assert.Equal("description", content.Description);
        Assert.Equal("VideoLink", content.VideoLink);
        Assert.Equal(3, content.Rating);
    }

    [Fact]
    public async Task ReadSectionAsync_returns_all_Sections()
    {
        var allSections = await _manager.GetSections();

        Assert.Collection(allSections,
            section => Assert.Equal(section.Id, _section.Id)
        );
    }

    [Fact]
    public async Task GetSection_by_id_returns_section()
    {
        var section = await _manager.GetSection(1);

        Assert.Equal(1, section.Id);
        Assert.Equal("python", section.Title);
        Assert.Equal("description", section.Description);
        Assert.Equal(_section.Content, section.Content);
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

    [Fact]
    public async Task GetContentInSection_returns_all_Content()
    {
        var allContent = await _manager.GetContentInSection(1);

        Assert.Collection(allContent,
            content => Assert.Equal(1, content.Id),
            content => Assert.Equal(2, content.Id),
            content => Assert.Equal(3, content.Id),
            content => Assert.Equal(4, content.Id)
        );
    }

    [Fact]
    public async Task UpdateSectionAsync_updates_existing_section()
    {
        var contentList = new List<Content>();
        var updateSection = new SectionUpdateDto
        {
            Title = "new title",
            Description = "description",
        };

        await _manager.UpdateSection(1, updateSection);

        var option = await _repository.GetSection(1);

        Assert.Equal(option.Value.Title, updateSection.Title);
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
