using System;
using System.Linq;
using SELearning.Core.User;

namespace SELearning.Infrastructure.Tests;

public class CommentManagerTests
{
    private readonly ICommentService _service;

    private static readonly Section.Section Section = new()
    {
        Id = 1,
        Title = "C#",
        Description = "C# tools",
        Content = new List<Content.Content>()
    };

    private static readonly User.User User = new() {Id = "ABC", Name = "Asger"};

    private static readonly Content.Content Content = new("Video on Entity Core", "Nice", "www.hej.dk", 1);

    private readonly IEnumerable<Comment.Comment> _comments = new List<Comment.Comment>
    {
        new("Nice", DateTime.Now, -10, Content, User),
        new("Cool but boring", DateTime.Now, 0, Content, User),
        new("This is a great video", DateTime.Now, 0, Content, User),
        new("Very inappropriate", DateTime.Now, 28, Content, User)
    };

    public CommentManagerTests()
    {
        //setting up the comment connection
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<SELearningContext>();
        builder.UseSqlite(connection);
        SELearningContext context = new(builder.Options);
        context.Database.EnsureCreated();

        ICommentRepository repo = new CommentRepository(context);
        _service = new CommentManager(repo);
        context.Content.Add(Content);

        Section.Content!.Add(Content);

        context.Comments.AddRange(
            _comments
        );

        context.SaveChanges();
    }

    [Fact]
    public async void Post_given_acceptable_input_does_post()
    {
        var dto = new CommentCreateDTO(User.ToUserDTO(), "Nice explanation", 1);
        await _service.PostComment(dto);

        Assert.Equal(User.ToUserDTO(), (await _service.GetCommentFromCommentId(5)).Author);
        Assert.Equal("Nice explanation", (await _service.GetCommentFromCommentId(5)).Text);
        Assert.Equal(1, (await _service.GetCommentFromCommentId(5)).ContentId);
        Assert.Equal(0, (await _service.GetCommentFromCommentId(5)).Rating);
    }

    [Fact]
    public async void Post_given_non_existing_content_throws_exception()
    {
        var dto = new CommentCreateDTO(new UserDTO("ABC", "Amalie"), "Nice explanation", 50);

        await Assert.ThrowsAsync<ContentNotFoundException>(() => _service.PostComment(dto));
    }

    [Fact]
    public async void Update_given_new_text_succeeds()
    {
        var dto = new CommentUpdateDTO("Very cool", -10);
        await _service.UpdateComment(1, dto);
        Assert.Equal("Very cool", (await _service.GetCommentFromCommentId(1)).Text);
    }

    [Fact]
    public async void Update_given_new_rating_succeeds()
    {
        var dto = new CommentUpdateDTO("Very inappropriate", 29);
        await _service.UpdateComment(4, dto);
        Assert.Equal(29, (await _service.GetCommentFromCommentId(4)).Rating);
    }

    [Fact]
    public async void Update_given_non_existing_id_throws_exception()
    {
        var dto = new CommentUpdateDTO("Very cool", 20);

        await Assert.ThrowsAsync<CommentNotFoundException>(() => _service.UpdateComment(6, dto));
    }

    [Fact]
    public async void Remove_given_existing_id_succeeds()
    {
        var comment = await _service.GetCommentFromCommentId(1);
        Assert.NotNull(comment);
        await _service.RemoveComment(1);
        await Assert.ThrowsAsync<CommentNotFoundException>(() => _service.GetCommentFromCommentId(1));
    }

    [Fact]
    public async void Remove_given_non_existing_id_throws_exception()
    {
        await Assert.ThrowsAsync<CommentNotFoundException>(() => _service.RemoveComment(7));
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
    public async void GetCommentsFromContentId_returns_all_comments_given_correct_contentId()
    {
        var comments = await _service.GetCommentsFromContentId(1);

        var commentDetailsDtos = comments.ToList();
        Assert.Contains(await _service.GetCommentFromCommentId(1), commentDetailsDtos);
        Assert.Contains(await _service.GetCommentFromCommentId(2), commentDetailsDtos);
        Assert.Contains(await _service.GetCommentFromCommentId(3), commentDetailsDtos);
        Assert.Contains(await _service.GetCommentFromCommentId(4), commentDetailsDtos);
        Assert.Equal(4, commentDetailsDtos.Count);
    }

    [Fact]
    public async void GetCommentsFromContentId_given_non_existing_contentId_throws_exception()
    {
        await Assert.ThrowsAsync<ContentNotFoundException>(() => _service.GetCommentsFromContentId(9));
    }

    [Fact]
    public async void GetCommentFromCommentId_given_existing_commentId_returns_comment()
    {
        var comment = await _service.GetCommentFromCommentId(2);

        Assert.Equal("Cool but boring", comment.Text);
        Assert.Equal(User.ToUserDTO(), comment.Author);
        Assert.Equal(1, comment.ContentId);
        Assert.Equal(2, comment.Id);
        Assert.Equal(0, comment.Rating);
    }

    [Fact]
    public async void GetCommentFromCommentId_given_non_existing_commentId_throws_exception()
    {
        await Assert.ThrowsAsync<CommentNotFoundException>(() => _service.GetCommentFromCommentId(9));
    }
}
