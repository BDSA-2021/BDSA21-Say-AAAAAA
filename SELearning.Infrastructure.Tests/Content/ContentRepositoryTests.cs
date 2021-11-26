using System;
using Microsoft.Data.Sqlite;
using Xunit;
using SELearning.Core.Content;
using System.Threading.Tasks;

namespace SELearning.Infrastructure.Tests;

public class ContentRepositoryTests : IDisposable
{

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

        var content = new Content();

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

        var created = await _repository.CreateAsync(content);

        Assert.NotNull(created.Id);
        Assert.Equal("section", created.Section);
        Assert.Equal("author", created.Author);
        Assert.Equal("title", created.Title);
        Assert.Equal("description", created.Description);
        Assert.Equal("video link", created.VideoLink);
        Assert.Equal(3, created.Rating);
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