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

    [Fact]
    public async Task CreateAsync_creates_new_content_with_generated_id()
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

        var (status, created) = await _repository.CreateContentAsync(content);

        Assert.NotNull(created.Id);
        Assert.Equal(_section, created.Section);
        Assert.Equal("author", created.Author);
        Assert.Equal("title", created.Title);
        Assert.Equal("description", created.Description);
        Assert.Equal("video link", created.VideoLink);
        Assert.Equal(3, created.Rating);
    }

    // [Fact]
    // public async Task CreateAsync_given_existing_Content_returns_Conflict_with_existing_Content()
    // {
    //     var content = new ContentCreateDto {
    //         Section = "section",
    //         Author = "author",
    //         Title = "title",
    //         Description = "description",
    //         VideoLink = "video link",
    //         Rating = 3,
    //     };

    //     // TODO: Hvorfor kan jeg ikke lave en DTO?
    //     var contentDto = new ContentDto {
    //         Id = 5,
    //         Section = "section",
    //         Author = "author",
    //         Title = "title",
    //         Description = "description",
    //         VideoLink = "video link",
    //         Rating = 3,
    //     };

    //     var (status, created) = await _repository.CreateAsync(content);

    //     Assert.Equal(contentDto, created);
    //     Assert.Equal(OperationResult.Conflict, status);
    // }

    [Fact]
    public async Task CreateAsync_given_Content_returns_Created_with_Content()
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

        var (status, created) = await _repository.CreateContentAsync(content);

        // TODO: Hvorfor kan jeg ikke lave en DTO?
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
    public async Task ReadAsync_given_non_existing_id_returns_None()
    {
        var option = await _repository.ReadContentAsync(42);

        Assert.True(option.IsNone);
    }

    [Fact]
    public async Task UpdateAsync_given_non_existing_id_returns_NotFound()
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

        var reponse = await _repository.UpdateContentAsync(42, content);

        Assert.Equal(OperationResult.NotFound, reponse);
    }

    [Fact]
    public async Task ReadAsync_given_existing_id_returns_Content()
    {
        var option = await _repository.ReadContentAsync(1);

        var content = new ContentDto { Id = 1, Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 };

        Assert.Equal(content, option.Value);
    }


    [Fact]
    public async Task ReadAsync_returns_all_content()
    {
    var allContent = await _repository.ReadContentAsync();
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
    public async Task UpdateAsync_updates_existing_content()
    {
        var content = new ContentUpdateDto { Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "video link", Rating = 3 };

        var updated = await _repository.UpdateContentAsync(1, content);

        Assert.Equal(OperationResult.Updated, updated);
    }

    [Fact]
    public async Task UpdateAsync_given_non_existing_Content_returns_NotFound()
    {
        var content = new ContentUpdateDto { Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "video link", Rating = 3 };

        var response = await _repository.UpdateContentAsync(42, content);

        Assert.Equal(OperationResult.NotFound, response);
    }


    [Fact]
    public async Task UpdateAsync_updates_and_returns_Updated()
    {
        var contentDto = new ContentUpdateDto {Section = _section, Author = "author", Title = "new title", Description = "description", VideoLink = "VideoLink", Rating = 3 };

        var response = await _repository.UpdateContentAsync(1, contentDto);

        var entity = await _context.Content.FirstAsync(c => c.Title == "new title");

        Assert.Equal(OperationResult.Updated, response);
    }

    [Fact]
    public async Task DeleteAsync_given_non_existing_Id_returns_NotFound()
    {
        var response = await _repository.DeleteContentAsync(42);

        Assert.Equal(OperationResult.NotFound, response);
    }

    [Fact]
    public async Task DeleteAsync_deletes_and_returns_Deleted()
    {
        var response = await _repository.DeleteContentAsync(2);

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