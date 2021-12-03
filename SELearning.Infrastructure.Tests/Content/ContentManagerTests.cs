using System;
using Microsoft.Data.Sqlite;
using Xunit;
using SELearning.Core.Content;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace SELearning.Infrastructure.Tests;

public class ContentManagerTests : IDisposable
{
    private readonly ContentContext _context;
    private readonly ContentRepository _repository;
    private readonly Section _section;
    private bool disposedValue;

    public ContentManagerTests()
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
    public async Task IncreaseContentRating_increase_existing_Content_rating_returns_updated()
    {
        var content = new ContentUpdateDto { Section = _section, Author = "author", Title = "title", Description = "description", VideoLink = "video link", Rating = 3 };

        var updated = await _repository.UpdateContent(1, content);

        Assert.Equal(OperationResult.Updated, updated);
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