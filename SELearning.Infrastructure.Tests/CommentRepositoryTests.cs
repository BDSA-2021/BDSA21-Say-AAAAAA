using System;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace SELearning.Infrastructure.Tests
{
    public class CommentRepositoryTests
    {
        CommentRepository _repository;
        CommentContext _context;

        public CommentRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<CommentContext>();
            builder.UseSqlite(connection);

            _repository = new CommentRepository();
            _context = new CommentContext(builder.Options);
        }
    }
}