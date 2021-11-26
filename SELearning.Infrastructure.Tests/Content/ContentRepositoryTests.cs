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
    private readonly ICollection<Guid> _idList;
    private readonly ContentContext _context;
    private readonly ContentRepository _repository;
    private bool disposedValue;

    public ContentRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ContentContext>();
        builder.UseSqlite(connection);
        var context = new ContentContext(builder.Options);
        context.Database.EnsureCreated();
        
        _idList = new List<Guid>();
        _idList.Add(new Guid());
        _idList.Add(new Guid());
        _idList.Add(new Guid());
        _idList.Add(new Guid());

        context.Content.AddRange(
            new Content { Id = 1, Section = "section", Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 },
            new Content { Id = 2, Section = "section", Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 },
            new Content { Id = 3, Section = "section", Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 },
            new Content { Id = 4, Section = "section", Author = "author", Title = "title", Description = "description", VideoLink = "VideoLink", Rating = 3 }
        );

        var section = new Section();

        context.SaveChanges();

        _context = context;
        _repository = new ContentRepository(_context);


    }

    [Fact]
    public async Task CreateAsync_creates_new_content_with_generated_id()
    {
        var content = new ContentCreateDto
        {
            Section = "section",
            Author = "author",
            Title = "title",
            Description = "description",
            VideoLink = "video link",
            Rating = 3,
        };

        var (status, created) = await _repository.CreateAsync(content);

        Assert.NotNull(created.Id);
        Assert.Equal("section", created.Section);
        Assert.Equal("author", created.Author);
        Assert.Equal("title", created.Title);
        Assert.Equal("description", created.Description);
        Assert.Equal("video link", created.VideoLink);
        Assert.Equal(3, created.Rating);
    }

    [Fact]
    public async Task UpdateAsync_given_non_existing_id_returns_NotFound()
    {
        var content = new ContentUpdateDto
        {
            Section = "section",
            Author = "author",
            Title = "title",
            Description = "description",
            VideoLink = "video link",
            Rating = 3,
        };

        var reponse = await _repository.UpdateAsync(42, content);

        Assert.Equal(OperationResult.NotFound, reponse);
    }


    [Fact]
    public async Task UpdateAsync_updates_existing_content()
    {
        var content = new ContentUpdateDto
        {
            Id = 1,
            Section = "section",
            Author = "author",
            Title = "title",
            Description = "description",
            VideoLink = "video link",
            Rating = 3,
        };

        var updated = await _repository.UpdateAsync(1, content);

        Assert.Equal(OperationResult.Updated, updated);

        // var option = await _repository.ReadAsync(1);
        // var superman = option.Value;

        // Assert.Empty(superman.Powers);
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