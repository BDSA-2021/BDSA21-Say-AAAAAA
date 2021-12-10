using System;
using SELearning.Core.User;
using System.Threading.Tasks;
using System.Linq;

namespace SELearning.Infrastructure.Tests;

public class ContentRepositoryTests
{
    private readonly SELearningContext _context;
    private readonly ContentRepository _repository;
    private readonly Section _section;
    private static readonly User _authorUser = new User
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
        var context = new SELearningContext(builder.Options);
        context.Database.EnsureCreated();

        _section = new Section { Id = 1, Title = "python", Description = "description" };

        var content1 = new Content("title", "description", "VideoLink", 3, _authorUser, _section);
        var content2 = new Content("title", "description", "VideoLink", 3, _authorUser, _section);
        var content3 = new Content("title", "description", "VideoLink", 3, _authorUser, _section);
        var content4 = new Content("title", "description", "VideoLink", 3, _authorUser, _section);

        var contentList = new List<Content>
        {
            content1,
            content2,
            content3,
            content4
        };

        _section.Content = contentList;

        context.Content.AddRange(contentList);
        context.Section.Add(_section);

        context.SaveChanges();

        _context = context;
        _repository = new ContentRepository(_context);
    }

    /*
        Section Tests Below
    */

    [Fact]
    public async Task CreateSectionAsync_creates_new_content_with_generated_id()
    {
        var section = new SectionCreateDto { Title = "title", Description = "description" };

        var created = (await _repository.AddSection(section)).Item2;

        Assert.NotNull(created.Id);
        Assert.Equal("title", created.Title);
        Assert.Equal("description", created.Description);
    }

    [Fact]
    public async Task CreateSectionAsync_given_Section_returns_Section_with_Section()
    {
        var section = new SectionCreateDto { Title = "title", Description = "description" };

        var (status, created) = await _repository.AddSection(section);

        var sectionDto = new SectionDto { Id = 2, Title = "title", Description = "description" };

        Assert.Equal(sectionDto.Id, created.Id);
        Assert.Equal(sectionDto.Title, created.Title);
        Assert.Equal(sectionDto.Description, created.Description);
        Assert.Equal(OperationResult.Created, status);
    }

    [Fact]
    public async Task ReadSectionAsync_given_non_existing_id_returns_None()
    {
        var option = await _repository.GetSection(42);

        Assert.True(option.IsNone);
    }

    [Fact]
    public async Task UpdateSectionAsync_given_non_existing_id_returns_NotFound()
    {
        var section = new SectionUpdateDto
        {
            Title = "title",
            Description = "description",
        };

        var reponse = await _repository.UpdateSection(42, section);

        Assert.Equal(OperationResult.NotFound, reponse);
    }

    [Fact]
    public async Task ReadSectionAsync_returns_all_Sections()
    {
        var allSections = await _repository.GetSections();

        Assert.Collection(allSections,
            section => Assert.Equal(section.Id, _section.Id)
        );
    }

    [Fact]
    public async Task UpdateSectionAsync_updates_existing_section()
    {
        var section = new SectionUpdateDto
        {
            Title = "title",
            Description = "description",
        };

        var updated = await _repository.UpdateSection(1, section);

        Assert.Equal(OperationResult.Updated, updated);
    }

    [Fact]
    public async Task UpdateSectionAsync_given_non_existing_Content_returns_NotFound()
    {
        var section = new SectionUpdateDto
        {
            Title = "title",
            Description = "description",
        };

        var response = await _repository.UpdateSection(42, section);

        Assert.Equal(OperationResult.NotFound, response);
    }

    [Fact]
    public async Task UpdateSectionAsync_updates_and_returns_Updated()
    {
        var contentList = new List<Content>();
        var section = new SectionUpdateDto
        {
            Title = "new title",
            Description = "description",
        };

        var response = await _repository.UpdateSection(1, section);

        var entity = await _context.Section.FirstAsync(c => c.Title == "new title");

        Assert.Equal(OperationResult.Updated, response);
    }

    [Fact]
    public async Task DeleteSectionAsync_given_non_existing_Id_returns_NotFound()
    {
        var response = await _repository.DeleteSection(42);

        Assert.Equal(OperationResult.NotFound, response);
    }

    [Fact]
    public async Task DeleteSectionAsync_deletes_and_returns_Deleted()
    {
        var response = await _repository.DeleteSection(1);

        var entity = await _context.Section.FindAsync(1);

        Assert.Equal(OperationResult.Deleted, response);
        Assert.Null(entity);
    }

    [Fact]
    public async Task GetContentInSection_returns_Content()
    {
        var contentInSection = await _repository.GetContentInSection(1);

        var content = from c in _section.Content
                      select new ContentDto
                      {
                          Id = c.Id,
                          Author = c.Author,
                          Title = c.Title,
                          Description = c.Description,
                          Section = c.Section,
                          VideoLink = c.VideoLink,
                          Rating = c.Rating
                      };

        Assert.Equal(content, contentInSection);
    }


    /*
        Contnet Tests Below
    */

    [Fact]
    public async Task CreateContentAsync_creates_new_content_with_generated_id()
    {
        var content = new ContentCreateDto
        {
            Title = "title",
            Description = "description",
            VideoLink = "video link",
            SectionId = _section.Id,
            Author = _authorUser
        };

        var created = (await _repository.AddContent(content)).Item2;

        Assert.Equal(_section, created.Section);
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
            Author = new User
            {
                Id = "Author",
                Name = "Author",
            }
        };

        var (status, created) = await _repository.AddContent(content);

        var contentDto = new ContentDto
        {
            Id = 5,
            Title = "title",
            Description = "description",
            VideoLink = "video link",
            Rating = 0,
            Section = _section,
            Author = new User
            {
                Id = "Author",
                Name = "Author",
            }
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
        var content = new ContentUpdateDto
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

        var content = new ContentDto { Id = 1, Section = _section, Author = new User { Id = "author", Name = "author" }, Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 };

        Assert.Equal(content, option.Value);
    }


    [Fact]
    public async Task ReadContentAsync_returns_all_content()
    {
        var allContent = await _repository.GetContent();

        Assert.All(allContent,
            content =>
            {
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
        var content = new ContentUpdateDto { Title = "title", Description = "description", VideoLink = "video link", Rating = 3 };

        var updated = await _repository.UpdateContent(1, content);

        Assert.Equal(OperationResult.Updated, updated);
    }

    [Fact]
    public async Task UpdateContentAsync_given_non_existing_Content_returns_NotFound()
    {
        var content = new ContentUpdateDto { Title = "title", Description = "description", VideoLink = "video link", Rating = 3 };

        var response = await _repository.UpdateContent(42, content);

        Assert.Equal(OperationResult.NotFound, response);
    }


    [Fact]
    public async Task UpdateContentAsync_updates_and_returns_Updated()
    {
        var contentDto = new ContentUpdateDto { Title = "new title", Description = "description", VideoLink = "VideoLink", Rating = 3 };

        var response = await _repository.UpdateContent(1, contentDto);

        var entity = await _context.Content.FirstAsync(c => c.Title == "new title");

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
        await _context.Content.AddRangeAsync(new[]
        {
            new Content("title", "description", "VideoLink", 3, _authorUser, _section),
            new Content("title", "description", "VideoLink", 3, new User { Id = "homer", Name = "homer" }, _section),
            new Content("title", "description", "VideoLink", 3, _authorUser, _section),
        });
        await _context.SaveChangesAsync();

        var contents = await _repository.GetContentByAuthor("homer");

        var expectedIds = new[] { 7 };
        var actualIds = contents.OrderBy(c => c.Id).Select(c => (int)c.Id!).ToList();
        Assert.Equal(expectedIds, actualIds);
    }
}
