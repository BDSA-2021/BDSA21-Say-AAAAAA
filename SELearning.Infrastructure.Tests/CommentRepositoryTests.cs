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

        public CommentRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<CommentContext>();
            builder.UseSqlite(connection);

            _context = new CommentContext(builder.Options);
            _context.Database.EnsureCreated();



            _repository = new CommentRepository(_context);

            Section section = new Section{
                Id = "1",
                Title = "C#",
                Description = "C# tools",
                Content = new List<Content>()
            };
            Content content = new Content{
                Author = "Sarah",
                Section = section,
                Id = "5",
                Title = "Video on Entity Core",
                Description = "Nice",
                VideoLink = "www.hej.dk"
            };
            section.Content.Add(content);

            _context.Comments.AddRange(
                new Comment { Author = "Amalie", Id = 1, Text = "Nice", Content = content },
                new Comment { Author = "Albert", Id = 2, Text = "Cool but boring", Content = content },
                new Comment { Author = "Paolo", Id = 3, Text = "This is a great video", Content = content },
                new Comment { Author = "Rasmus", Id = 4, Text = "Very inappropriate", Content = content }
            );

            _context.SaveChanges();
        }

        [Fact]
        public async Task AddComment_creates_new_comment_with_generated_id()
        {
            CommentCreateDTO comment = new CommentCreateDTO("Harleen", "Nice content","1");

            var created = await _repository.AddComment(comment);

            Assert.Equal(5, created.Item2.Id);
            Assert.Equal("Harleen", created.Item2.Author);
            Assert.Equal("Nice content", created.Item2.Text);
            Assert.Equal(OperationResult.Created,created.Item1);
        }

        [Fact]
        public async Task AddComment__given_non_existing_ContentId_returns_NotFound()
        {
            CommentCreateDTO comment = new CommentCreateDTO("Harleen", "Nice content","2");

            var created = await _repository.AddComment(comment); 

            Assert.Equal(OperationResult.NotFound,created.Item1);   
        }

        [Fact]
        public async Task UpdateComment_given_non_existing_id_returns_NotFound()
        {
            CommentUpdateDTO dto = new CommentUpdateDTO("Really like this content");

            var updated = await _repository.UpdateComment(42, dto);

            Assert.Equal(OperationResult.NotFound, updated.Item1);
        }

        [Fact]
        public async Task UpdateComment_updates_existing_comment()
        {
            CommentUpdateDTO dto = new CommentUpdateDTO("Nice but also confusing");

            var updated = await _repository.UpdateComment(1, dto);

            Assert.Equal(1, updated.Item2.Id);
            Assert.Equal("Amalie", updated.Item2.Author);
            Assert.Equal("Nice but also confusing", updated.Item2.Text);
            
            Assert.Equal(OperationResult.Updated, updated.Item1);
        }
    }
}