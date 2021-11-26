using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using SELearning.Core.Comment;
using System.Threading.Tasks;
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
                new Comment { Author = "Amalie", Id = 1, Text = "Nice", Content = 4 },
                new Comment { Author = "Albert", Id = 2, Text = "Cool but boring", Content = 4 },
                new Comment { Author = "Paolo", Id = 3, Text = "This is a great video", Content = 3 },
                new Comment { Author = "Rasmus", Id = 4, Text = "Very inappropriate", Content = 5 }
            );

            _context.SaveChanges();
        }

        [Fact]
        public async Task AddComment_creates_new_comment_with_generated_id()
        {
            CommentDTO comment = new CommentDTO("Harleen", "Nice content");

            var created = await _repository.AddComment(comment);

            Assert.Equal(5, created.Item2.Id);
            Assert.Equal("Harleen", created.Item2.Author);
            Assert.Equal("Nice content", created.Item2.Text);
        }

        [Fact]
        public async Task UpdateComment_given_non_existing_id_returns_NotFound()
        {
            CommentDTO dto = new CommentDTO("Amalie","Really like this content");

            var updated = await _repository.UpdateComment(42, dto);

            Assert.Equal(OperationResult.NotFound, updated.Item1);
        }
    }
}