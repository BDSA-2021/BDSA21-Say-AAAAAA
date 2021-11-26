using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using SELearning.Core;

namespace SELearning.Infrastructure.Tests
{
    public class CommentRepositoryTests
    {
        private readonly CommentRepository _repository;
        private readonly CommentContext _context;

        public CommentRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<CommentContext>();
            builder.UseSqlite(connection);

            _context = new CommentContext(builder.Options);
            _context.Database.EnsureCreated();

            _repository = new CommentRepository(_context);

            _context.Comments.AddRange(
                new Comment {Author = "Amalie",Id = 1, Content = "Nice", ContentId = 4},
                new Comment {Author = "Albert",Id = 2, Content = "Cool but boring", ContentId = 4},
                new Comment {Author = "Paolo",Id = 3, Content = "This is a great video", ContentId = 3},
                new Comment {Author = "Rasmus",Id = 4, Content = "Very inappropriate", ContentId = 5}
            );

            _context.SaveChanges();
        }
    }
}