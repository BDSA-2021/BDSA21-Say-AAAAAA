using System;
using Microsoft.Data.Sqlite;
using Xunit;
using SELearning.Core.Content;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace SELearning.Infrastructure.Tests;

public class ContentRepositoryTests : IDisposable
{
    private readonly ContentContext _context;
    private readonly ContentRepository _repository;
    private readonly Section _section;
    private bool disposedValue;

    public ContentRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ContentContext>();
        builder.UseSqlite(connection);
        var context = new ContentContext(builder.Options);
        context.Database.EnsureCreated();

        _section = new Section {Id = 1, Title = "python", Description = "description"};

        var content1 = new Content { Id = 1, Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 };
        var content2 = new Content { Id = 2, Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 };
        var content3 = new Content { Id = 3, Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 };
        var content4 = new Content { Id = 4, Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 };

        var contentList = new List<Content>();
        contentList.Add(content1);
        contentList.Add(content2);
        contentList.Add(content3);
        contentList.Add(content4);

        _section.Content = contentList;

        context.Content.AddRange(
            content1,
            content2,
            content3,
            content4
        );

        context.Section.AddRange(
            _section
        );

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
        var contentList = new List<Content>();
        var section = new SectionCreateDto { Title = "title", Description = "description", Content = contentList };

        var (status, created) = await _repository.AddSection(section);

        Assert.NotNull(created.Id);
        Assert.Equal("title", created.Title);
        Assert.Equal("description", created.Description);
        Assert.Equal(contentList, created.Content);
    }

    [Fact]
    public async Task CreateSectionAsync_given_Section_returns_Section_with_Section()
    {
        var contentList = new List<Content>();
        var section = new SectionCreateDto { Title = "title", Description = "description", Content = contentList };

        var (status, created) = await _repository.AddSection(section);

        var sectionDto = new SectionDto { Id = 2, Title = "title", Description = "description", Content = contentList };

        Assert.Equal(sectionDto, created);
        Assert.Equal(OperationResult.Created, status);
    }

    [Fact]
    public async Task ReadSectionAsync_given_non_existing_id_returns_None()
    {
        var option = await _repository.ReadSectionAsync(42);

        Assert.True(option.IsNone);
    }

    [Fact]
    public async Task UpdateSectionAsync_given_non_existing_id_returns_NotFound()
    {
        var contentList = new List<Content>();
        var section = new SectionUpdateDto
        {
            Title = "title",
            Description = "description",
            Content = contentList
        };

        var reponse = await _repository.UpdateSection(42, section);

        Assert.Equal(OperationResult.NotFound, reponse);
    }

    // [Fact]
    // public async Task ReadSectionAsync_given_existing_id_returns_Section()
    // {
    //     var option = await _repository.ReadSectionAsync(1);

    //     var contentList = new List<Content>();
    //     var section = new SectionCreateDto { Title = "title", Description = "description", Content = contentList };

    //     Assert.Equal(section, option.Value);
    // }

    [Fact]
    public async Task ReadSectionAsync_returns_all_Sections()
    {
    var allSections = await _repository.ReadSectionAsync();

        Assert.Collection(allSections,
            section => Assert.Equal(section.Id, _section.Id)
        );
    }

    [Fact]
    public async Task UpdateSectionAsync_updates_existing_section()
    {
        var contentList = new List<Content>();
        var section = new SectionUpdateDto
        {
            Title = "title",
            Description = "description",
            Content = contentList
        };

        var updated = await _repository.UpdateSection(1, section);

        Assert.Equal(OperationResult.Updated, updated);
    }

    [Fact]
    public async Task UpdateSectionAsync_given_non_existing_Content_returns_NotFound()
    {
        var contentList = new List<Content>();
        var section = new SectionUpdateDto
        {
            Title = "title",
            Description = "description",
            Content = contentList
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
            Content = contentList
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
                         select new ContentDto {
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
            Section = _section,
            Author = "author",
            Title = "title",
            Description = "description",
            VideoLink = "video link",
            Rating = 3,
        };

        var (status, created) = await _repository.AddContent(content);

        Assert.NotNull(created.Id);
        Assert.Equal(_section, created.Section);
        Assert.Equal("author", created.Author);
        Assert.Equal("title", created.Title);
        Assert.Equal("description", created.Description);
        Assert.Equal("video link", created.VideoLink);
        Assert.Equal(3, created.Rating);
    }

    [Fact]
    public async Task CreateContentAsync_given_Content_returns_Created_with_Content()
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

        var (status, created) = await _repository.AddContent(content);

        var contentDto = new ContentDto {
            Id = 5,
            Section = _section,
            Author = "author",
            Title = "title",
            Description = "description",
            VideoLink = "video link",
            Rating = 3,
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
            Section = _section,
            Author = "author",
            Title = "title",
            Description = "description",
            VideoLink = "video link",
            Rating = 3,
        };

        var reponse = await _repository.UpdateContent(42, content);

        Assert.Equal(OperationResult.NotFound, reponse);
    }

    [Fact]
    public async Task ReadContentAsync_given_existing_id_returns_Content()
    {
        var option = await _repository.GetContent(1);

        var content = new ContentDto { Id = 1, Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 };

        Assert.Equal(content, option.Value);
    }


    [Fact]
    public async Task ReadContentAsync_returns_all_content()
    {
    var allContent = await _repository.GetContent();
        var contentDto1 = new ContentDto {Id = 1, Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 };
        var contentDto2 = new ContentDto {Id = 2, Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 };
        var contentDto3 = new ContentDto {Id = 3, Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 };
        var contentDto4 = new ContentDto {Id = 4, Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 };

        Assert.Collection(allContent,
            content => Assert.Equal(contentDto1, content),
            content => Assert.Equal(contentDto2, content),
            content => Assert.Equal(contentDto3, content),
            content => Assert.Equal(contentDto4, content)
        );
    }


    [Fact]
    public async Task UpdateContentAsync_updates_existing_content()
    {
        var content = new ContentUpdateDto { Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "video link", Rating = 3 };

        var updated = await _repository.UpdateContent(1, content);

        Assert.Equal(OperationResult.Updated, updated);
    }

    [Fact]
    public async Task UpdateContentAsync_given_non_existing_Content_returns_NotFound()
    {
        var content = new ContentUpdateDto { Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "video link", Rating = 3 };

        var response = await _repository.UpdateContent(42, content);

        Assert.Equal(OperationResult.NotFound, response);
    }


    [Fact]
    public async Task UpdateContentAsync_updates_and_returns_Updated()
    {
        var contentDto = new ContentUpdateDto {Section = _section, Author = "author", Title = "new title", Description = "description", VideoLink = "VideoLink", Rating = 3 };

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