using System.Threading.Tasks;
using System.Linq;
using SELearning.Infrastructure.Section;

namespace SELearning.Infrastructure.Tests;

public class SectionRepositoryTests
{
    private readonly SELearningContext _context;
    private readonly SectionRepository _repository;

    private static readonly Section.Section Section = new() {Id = 1, Title = "python", Description = "description"};

    private static readonly Section.Section
        SectionEmpty = new() {Id = 2, Title = "python", Description = "description"};

    private static readonly User.User User = new() {Id = "ABC", Name = "Adrian"};

    public SectionRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<SELearningContext>();
        builder.UseSqlite(connection);
        var context = new SELearningContext(builder.Options);
        context.Database.EnsureCreated();

        var content1 = new Content.Content("title", "description", "VideoLink", 3, User, Section);
        var content2 = new Content.Content("title", "description", "VideoLink", 3, User, Section);
        var content3 = new Content.Content("title", "description", "VideoLink", 3, User, Section);
        var content4 = new Content.Content("title", "description", "VideoLink", 3, User, Section);

        var contentList = new List<Content.Content>
        {
            content1,
            content2,
            content3,
            content4
        };

        Section.Content = contentList;

        context.Content.AddRange(
            content1,
            content2,
            content3,
            content4
        );

        context.Section.AddRange(
            Section,
            SectionEmpty
        );

        context.SaveChanges();

        _context = context;
        _repository = new SectionRepository(_context);
    }

    /*
        Section Tests Below
    */

    [Fact]
    public async Task CreateSectionAsync_creates_new_content_with_generated_id()
    {
        var section = new SectionCreateDTO {Title = "title", Description = "description"};

        var created = (await _repository.AddSection(section)).Item2;

        Assert.NotNull(created.Id);
        Assert.Equal("title", created.Title);
        Assert.Equal("description", created.Description);
    }

    [Fact]
    public async Task CreateSectionAsync_given_Section_returns_Section_with_Section()
    {
        var section = new SectionCreateDTO {Title = "title", Description = "description"};

        var (status, created) = await _repository.AddSection(section);

        var sectionDto = new SectionDTO {Id = 3, Title = "title", Description = "description"};

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
        var section = new SectionUpdateDTO
        {
            Title = "title",
            Description = "description"
        };

        var reponse = await _repository.UpdateSection(42, section);

        Assert.Equal(OperationResult.NotFound, reponse);
    }

    [Fact]
    public async Task ReadSectionAsync_returns_all_Sections()
    {
        var allSections = await _repository.GetSections();

        Assert.Collection(allSections,
            section => Assert.Equal(1, section.Id),
            section => Assert.Equal(2, section.Id)
        );
    }

    [Fact]
    public async Task UpdateSectionAsync_updates_existing_section()
    {
        var section = new SectionUpdateDTO
        {
            Title = "title",
            Description = "description"
        };

        var updated = await _repository.UpdateSection(1, section);

        Assert.Equal(OperationResult.Updated, updated);
    }

    [Fact]
    public async Task UpdateSectionAsync_given_non_existing_Content_returns_NotFound()
    {
        var section = new SectionUpdateDTO
        {
            Title = "title",
            Description = "description"
        };

        var response = await _repository.UpdateSection(42, section);

        Assert.Equal(OperationResult.NotFound, response);
    }

    [Fact]
    public async Task UpdateSectionAsync_updates_and_returns_Updated()
    {
        var section = new SectionUpdateDTO
        {
            Title = "new title",
            Description = "description"
        };

        var response = await _repository.UpdateSection(1, section);

        await _context.Section.FirstAsync(c => c.Title == "new title");

        Assert.Equal(OperationResult.Updated, response);
    }

    [Fact]
    public async Task DeleteSectionAsync_given_non_existing_Id_returns_NotFound()
    {
        var response = await _repository.DeleteSection(42);

        Assert.Equal(OperationResult.NotFound, response);
    }

    [Fact]
    public async Task DeleteSection_with_content_returns_Conflict()
    {
        var response = await _repository.DeleteSection(1);

        Assert.Equal(OperationResult.Conflict, response);
    }

    [Fact]
    public async Task DeleteSectionAsync_deletes_and_returns_Deleted()
    {
        var response = await _repository.DeleteSection(2);

        var entity = await _context.Section.FindAsync(2);

        Assert.Equal(OperationResult.Deleted, response);
        Assert.Null(entity);
    }

    [Fact]
    public async Task GetContentInSection_returns_Content()
    {
        var contentInSection = await _repository.GetContentInSection(1);

        var content = from c in Section.Content
            select new ContentDTO
            {
                Id = c.Id,
                Author = c.Author.ToUserDTO(),
                Title = c.Title,
                Description = c.Description,
                Section = c.Section.ToSectionDTO(),
                VideoLink = c.VideoLink,
                Rating = c.Rating
            };

        Assert.Equal(content, contentInSection);
    }
}
