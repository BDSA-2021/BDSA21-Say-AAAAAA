using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using SELearning.Core.Comment;
using System.Threading.Tasks;
using SELearning.Core;
using System.Collections.Generic;

namespace SELearning.Infrastructure.Tests
{
    public class CommentRepositoryTests
    {
        private readonly CommentRepository _repository;
        private readonly CommentContext _context;

        private static readonly Section section = new Section
        {
            Id = "1",
            Title = "C#",
            Description = "C# tools",
            Content = new List<Content>()
        };
        private static readonly Content content = new Content
        {
            Author = "Sarah",
            Section = section,
            Id = 1,
            Title = "Video on Entity Core",
            Description = "Nice",
            VideoLink = "www.hej.dk"
        };
        private IEnumerable<Comment> _comments = new List<Comment>()
        {
            new Comment { Author = "Amalie", Id = 1, Text = "Nice", Content = content },
                new Comment { Author = "Albert", Id = 2, Text = "Cool but boring", Content = content },
                new Comment { Author = "Paolo", Id = 3, Text = "This is a great video", Content = content },
                new Comment { Author = "Rasmus", Id = 4, Text = "Very inappropriate", Content = content }
        };

        public CommentRepositoryTests()
        {
            //setting up the comment connection
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<CommentContext>();
            builder.UseSqlite(connection);
            _context = new CommentContext(builder.Options);
            _context.Database.EnsureCreated();

            _repository = new CommentRepository(_context);

            section.Content.Add(content);

            _context.Comments.AddRange(
                _comments
            );

            _context.SaveChanges();
        }

        [Fact]
        public async Task AddComment_creates_new_comment_with_generated_id()
        {
            CommentCreateDTO comment = new CommentCreateDTO("Harleen", "Nice content", 1);

            var created = await _repository.AddComment(comment);

            Assert.Equal(5, created.Item2.Id);
            Assert.Equal("Harleen", created.Item2.Author);
            Assert.Equal("Nice content", created.Item2.Text);
            Assert.Equal(OperationResult.Created, created.Item1);
        }

        [Fact]
        public async Task AddComment__given_non_existing_ContentId_returns_NotFound()
        {
            CommentCreateDTO comment = new CommentCreateDTO("Harleen", "Nice content", 2);

            var created = await _repository.AddComment(comment);

            Assert.Equal(OperationResult.NotFound, created.Item1);
        }

        [Fact]
        public async Task UpdateComment_given_non_existing_id_returns_NotFound()
        {
            CommentUpdateDTO dto = new CommentUpdateDTO("Really like this content", 0);

            var updated = await _repository.UpdateComment(42, dto);

            Assert.Equal(OperationResult.NotFound, updated.Item1);
        }

        [Fact]
        public async Task UpdateComment_updates_existing_comment()
        {
            CommentUpdateDTO dto = new CommentUpdateDTO("Nice but also confusing", 1);

            var updated = await _repository.UpdateComment(1, dto);

            Assert.Equal(1, updated.Item2.Id);
            Assert.Equal("Amalie", updated.Item2.Author);
            Assert.Equal("Nice but also confusing", updated.Item2.Text);
            Assert.Equal(1, updated.Item2.Rating);

            Assert.Equal(OperationResult.Updated, updated.Item1);
        }

        [Fact]
        public async Task RemoveComment_given_not_existing_id_returns_NotFound()
        {
            var removed = await _repository.RemoveComment(68);

            Assert.Equal(OperationResult.NotFound, removed);
        }

        [Fact]
        public async Task RemoveComment_given_existing_id_removes_comment()
        {
            var removed = await _repository.RemoveComment(2);

            Assert.Equal(OperationResult.Deleted, removed);

            var tryRead = await _repository.GetCommentByCommentId(2);
            Assert.Null(tryRead.Value);
        }

        [Fact]
        public async Task GetCommentByCommentId_given_existing_id_returns_comment()
        {
            var read = (await _repository.GetCommentByCommentId(3)).Value;

            Assert.Equal("Paolo", read.Author);
            Assert.Equal("This is a great video", read.Text);
        }

        [Fact]
        public async Task GetCommentByCommentId_given_not_existing_id_returns_null()
        {
            var read = await _repository.GetCommentByCommentId(90);

            Assert.Null(read.Value);
        }

        [Fact]
        public async Task GetCommentsByContentId_given_existing_id_returns_comments()
        {
            var read = await _repository.GetCommentsByContentId(1);

            Assert.Equal(OperationResult.Succes, read.Item2);
            Assert.Equal(_comments, read.Item1);
        }

        [Fact]
        public async Task GetCommentsByContentId_given_not_existing_id_returns_NotFound()
        {
            var read = await _repository.GetCommentsByContentId(90);

            Assert.Equal(OperationResult.NotFound, read.Item2);
        }
    }
}