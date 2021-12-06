using SELearning.Core.Comment;
using SELearning.Core;
using Xunit;
using System;
using System.Collections.Generic;

namespace SELearning.Infrastructure.Tests
{
    public class CommentManagerTests
    {
        ICommentService _service;
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
            new Comment { Author = "Amalie", Id = 1, Text = "Nice", Content = content, Rating = -10 },
                new Comment { Author = "Albert", Id = 2, Text = "Cool but boring", Content = content },
                new Comment { Author = "Paolo", Id = 3, Text = "This is a great video", Content = content },
                new Comment { Author = "Rasmus", Id = 4, Text = "Very inappropriate", Content = content, Rating = 28 }
        };
        public CommentManagerTests()
        {
            //setting up the comment connection
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<CommentContext>();
            builder.UseSqlite(connection);
            CommentContext _context = new CommentContext(builder.Options);
            _context.Database.EnsureCreated();

            ICommentRepository _repo = new CommentRepository(_context);
            _service = new CommentManager(_repo);

            section.Content.Add(content);

            _context.Comments.AddRange(
                _comments
            );

            _context.SaveChanges();
        }

        public void Post_given_acceptable_input_does_post()
        {

        }


        public void Post_given_non_existing_content_throws_exception()
        {

        }

        [Fact]
        public void Update_given_new_text_succeeds()
        {

        }

        [Fact]
        public void Update_given_non_existing_id_throws_exception()
        {

        }

        [Fact]
        public void Remove_given_existing_id_succeeds()
        {

        }

        [Fact]
        public void Remove_given_non_existing_id_throws_exception()
        {

        }

        [Fact]
        public async void Upvote_plusses_1_given_zero_rating()
        {
            await _service.UpvoteComment(3);

            Assert.Equal(1, (await _service.GetCommentFromCommentId(3)).Rating);
        }

        [Fact]
        public async void Upvote_plusses_1_given_negative_rating()
        {
            await _service.UpvoteComment(1);
            Assert.Equal(-9, (await _service.GetCommentFromCommentId(1)).Rating);

            await _service.UpvoteComment(1);
            await _service.UpvoteComment(1);
            await _service.UpvoteComment(1);
            await _service.UpvoteComment(1);
            Assert.Equal(-5, (await _service.GetCommentFromCommentId(1)).Rating);
        }

        [Fact]
        public async void Upvote_plusses_1_given_positive_rating()
        {
            await _service.UpvoteComment(4);
            Assert.Equal(29, (await _service.GetCommentFromCommentId(4)).Rating);
        }

        [Fact]
        public async void Downvote_subtracts_1_given_zero_rating()
        {
            await _service.DownvoteComment(3);

            Assert.Equal(-1, (await _service.GetCommentFromCommentId(3)).Rating);
        }

        [Fact]
        public async void Downvote_subtracts_1_given_negative_rating()
        {
            await _service.DownvoteComment(1);
            Assert.Equal(-11, (await _service.GetCommentFromCommentId(1)).Rating);

            await _service.DownvoteComment(1);
            await _service.DownvoteComment(1);
            await _service.DownvoteComment(1);
            await _service.DownvoteComment(1);
            Assert.Equal(-15, (await _service.GetCommentFromCommentId(1)).Rating);
        }

        [Fact]
        public async void Downvote_subtracts_1_given_positive_rating()
        {
            await _service.DownvoteComment(4);
            Assert.Equal(27, (await _service.GetCommentFromCommentId(4)).Rating);
        }

        [Fact]
        public void GetCommentsFromContentId_returns_all_comments_given_correct_contentId()
        {
            /*Comment cmt = new Comment
            {
                Author = "Ida",
                Content = "Really like this",
                Id = 3,
                Rating = 7,
                ContentId = 6
            };

            List<Comment> comments = _service.GetCommentsFromContentId(6);

            Assert.Contains(cmt, comments);
            Assert.Equal(1, comments.Count);*/
        }

        [Fact]
        public void GetCommentsFromContentId_given_non_existing_contentId_throws_exception()
        {

        }

        [Fact]
        public void GetCommentFromCommentId_given_existing_commentId_returns_comment()
        {

        }

        [Fact]
        public void GetCommentFromCommentId_given_non_existing_commentId_throws_exception()
        {

        }
    }
}