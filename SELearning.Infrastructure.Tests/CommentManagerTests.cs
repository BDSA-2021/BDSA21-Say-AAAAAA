using SELearning.Core.Services;
using SELearning.Core;
using Xunit;
using System;
using System.Collections.Generic;

namespace SELearning.Infrastructure.Tests
{
    public class CommentManagerTests
    {
        ICommentService _service = new CommentManager();

        [Theory]
        [InlineData("Amalie", "A really nice and professional video!")]
        [InlineData("Y84Gmig", "Found this very confusing...")]
        [InlineData("Anonymous user from ITU", "Cool explanation, but i didn't exactly get the part on regression testing. Can someone elaborate?")]
        public void Post_given_acceptable_input_does_post(string author, string content)
        {

            _service.PostComment(author, content);
            //TODO: how do i check that this has actually been done?
        }

        [Theory]
        [InlineData("Amalie", "")]
        [InlineData("Y84Gmig", "    ")]
        [InlineData("", "")]
        public void Post_given_empty_content_throws_exception(string author, string content)
        {
            _service.PostComment(author, content);

            //TODO: how do i assert that it throws something and what do we want it to throw?
        }

        [Fact]
        public void Update_given_new_content_succeeds()
        {
            //TODO: how to do this? 
        }

        [Fact]
        public void Remove_given_comment_succeeds()
        {
            //TODO: how to do this? 
        }

        [Fact]
        public void Upvote_plusses_1_given_zero_rating()
        {
            Comment cmt = new Comment
            {
                Author = "Ida",
                Content = "Really like this",
                Id = 3
            };
            _service.UpvoteComment(cmt);

            Assert.Equal(1, cmt.Rating);
        }

        [Fact]
        public void Upvote_plusses_1_given_negative_rating()
        {
            Comment cmt = new Comment
            {
                Author = "Ida",
                Content = "Really like this",
                Id = 3,
                Rating = -10
            };

            _service.UpvoteComment(cmt);
            Assert.Equal(-9, cmt.Rating);

            _service.UpvoteComment(cmt);
            _service.UpvoteComment(cmt);
            _service.UpvoteComment(cmt);
            Assert.Equal(-6, cmt.Rating);
        }

        [Fact]
        public void Upvote_plusses_1_given_positive_rating()
        {
            Comment cmt = new Comment
            {
                Author = "Ida",
                Content = "Really like this",
                Id = 3,
                Rating = 7
            };

            _service.UpvoteComment(cmt);
            Assert.Equal(8, cmt.Rating);

            _service.UpvoteComment(cmt);
            _service.UpvoteComment(cmt);
            _service.UpvoteComment(cmt);
            Assert.Equal(11, cmt.Rating);
        }

        [Fact]
        public void Downvote_subtracts_1_given_zero_rating()
        {
            Comment cmt = new Comment
            {
                Author = "Ida",
                Content = "Really like this",
                Id = 3
            };
            _service.DownvoteComment(cmt);

            Assert.Equal(-1, cmt.Rating);
        }

        [Fact]
        public void Downvote_subtracts_1_given_negative_rating()
        {
            Comment cmt = new Comment
            {
                Author = "Ida",
                Content = "Really like this",
                Id = 3,
                Rating = -10
            };

            _service.DownvoteComment(cmt);
            Assert.Equal(-11, cmt.Rating);

            _service.DownvoteComment(cmt);
            _service.DownvoteComment(cmt);
            _service.DownvoteComment(cmt);
            Assert.Equal(-14, cmt.Rating);
        }

        [Fact]
        public void Downvote_subtracts_1_given_positive_rating()
        {
            Comment cmt = new Comment
            {
                Author = "Ida",
                Content = "Really like this",
                Id = 3,
                Rating = 7
            };

            _service.DownvoteComment(cmt);
            Assert.Equal(6, cmt.Rating);

            _service.DownvoteComment(cmt);
            _service.DownvoteComment(cmt);
            _service.DownvoteComment(cmt);
            Assert.Equal(3, cmt.Rating);
        }

        /* [Fact]
         public void GetCommentsFromContentId_returns_all_comments_given_correct_contentId()
         {
             Comment cmt = new Comment
             {
                 Author = "Ida",
                 Content = "Really like this",
                 Id = 3,
                 Rating = 7,
                 ContentId = 6
             };

             List<Comment> comments = _service.GetCommentsFromContentId(6);

             Assert.Contains(cmt, comments);
             Assert.Equal(1, comments.Count);
         }*/

    }
}