using System;
using Microsoft.Data.Sqlite;
using Xunit;
using SELearning.Core.Content;

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


    public void Dispose()
    {
        throw new NotImplementedException();
    }
}