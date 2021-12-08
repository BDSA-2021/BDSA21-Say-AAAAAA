using System.Linq;

namespace SELearning.Infrastructure.Tests;

public class CommentManagerTests
{
    readonly ICommentService _service;

    private static readonly Section section = new()
    {
        Id = 1,
        Title = "C#",
        Description = "C# tools",
        Content = new List<Content>()
    };

    private static readonly Content content = new()
    {
        Author = "Sarah",
        Section = section,
        Id = 1,
        Title = "Video on Entity Core",
        Description = "Nice",
        VideoLink = "www.hej.dk"
    };

    private readonly IEnumerable<Comment> _comments = new List<Comment>()
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
        var builder = new DbContextOptionsBuilder<SELearningContext>();
        builder.UseSqlite(connection);
        SELearningContext _context = new(builder.Options);
        _context.Database.EnsureCreated();

        ICommentRepository _repo = new CommentRepository(_context);
        _service = new CommentManager(_repo);

        section.Content!.Add(content);

        _context.Comments.AddRange(
            _comments
        );

        _context.SaveChanges();
    }

    [Fact]
    public async void Post_given_acceptable_input_does_post()
    {
        var dto = new CommentCreateDTO("Christine", "Nice explanation", 1);
        await _service.PostComment(dto);
        Assert.Equal("Christine", (await _service.GetCommentFromCommentId(5)).Author);
        Assert.Equal("Nice explanation", (await _service.GetCommentFromCommentId(5)).Text);
        Assert.Equal(1, (await _service.GetCommentFromCommentId(5)).ContentId);
        Assert.Equal(0, (await _service.GetCommentFromCommentId(5)).Rating);
    }

    [Fact]
    public async void Post_given_non_existing_content_throws_exception()
    {
        var dto = new CommentCreateDTO("Amalie", "Nice explanation", 50);

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
        CommentDetailsDTO comment = await _service.GetCommentFromCommentId(1);
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
        IEnumerable<CommentDetailsDTO> comments = await _service.GetCommentsFromContentId(1);

        Assert.Contains((await _service.GetCommentFromCommentId(1)), comments);
        Assert.Contains((await _service.GetCommentFromCommentId(2)), comments);
        Assert.Contains((await _service.GetCommentFromCommentId(3)), comments);
        Assert.Contains((await _service.GetCommentFromCommentId(4)), comments);
        Assert.Equal(4, comments.Count());
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
        Assert.Equal("Albert", comment.Author);
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
