namespace SELearning.API.Tests;

public class CommentControllerTest
{
    private readonly CommentController _controller;
    private readonly Mock<ICommentService> _service;
    private readonly Mock<IAuthorizationService> _auth;
    private readonly UserDTO _user;

    public CommentControllerTest()
    {
        var logger = new Mock<ILogger<CommentController>>();

        _user = new UserDTO("ABC", "Joachim");

        _auth = new Mock<IAuthorizationService>();
        _auth.Setup(x => x.AuthorizeAsync(It.IsNotNull<ClaimsPrincipal>(), It.Is<object>(x => x is IAuthored), It.IsNotNull<string>()))
            .ReturnsAsync(AuthorizationResult.Success);

        _service = new Mock<ICommentService>();
        _service.Setup(x => x.GetCommentFromCommentId(It.Is<int>(x => x != 0)))
                .ReturnsAsync(new CommentDetailsDTO(_user, "Hej", 1, DateTime.Now, 100, 1));
        _service.Setup(m => m.PostComment(It.Is<CommentCreateDTO>(x => x.ContentId <= 0)))
                .ThrowsAsync(new ContentNotFoundException(-1));

        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(x => x.GetOrAddUser(It.IsNotNull<UserDTO>())).ReturnsAsync(_user);

        _controller = new CommentController(logger.Object, _service.Object, userRepo.Object, _auth.Object);
        _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() };
    }

    [Fact]
    public async Task GetComment_Given_Valid_ID_Returns_Comment()
    {
        // Arrange
        var expected = new CommentDetailsDTO(_user, "Hallooooo", 1, DateTime.Now, 1000, 0);
        _service.Setup(m => m.GetCommentFromCommentId(1)).ReturnsAsync(expected);

        // Act
        var actual = ((await _controller.GetComment(1)).Result as OkObjectResult)!.Value;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetComment_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        _service.Setup(m => m.GetCommentFromCommentId(-1)).ThrowsAsync(new CommentNotFoundException(-1));

        // Act
        var response = (await _controller.GetComment(-1)).Result;

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task GetCommentsByContentID_Given_Valid_ID_Returns_Comments()
    {
        // Arrange
        var expected = new List<CommentDetailsDTO>();
        _service.Setup(m => m.GetCommentsFromContentId(1)).ReturnsAsync(expected);

        // Act
        var actual = ((await _controller.GetCommentsByContentID(1)).Result as OkObjectResult)!.Value;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetCommentsByContentID_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        _service.Setup(m => m.GetCommentsFromContentId(-1)).ThrowsAsync(new ContentNotFoundException(-1));

        // Act
        var response = (await _controller.GetCommentsByContentID(-1)).Result;

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task CreateComment_Given_CommentCreateDTO_With_Valid_ContentID_Returns_CreatedAtRoute()
    {
        // Arrange
        var toCreate = new CommentUserDTO(1, "Text");
        var expected = new CommentDetailsDTO(_user, "Text", 1, DateTime.Now, 0, default!);
        _service.Setup(m => m.PostComment(It.IsNotNull<CommentCreateDTO>())).ReturnsAsync(expected);

        // Act
        var actual = (await _controller.CreateComment(toCreate) as CreatedAtActionResult)!;

        // Assert
        Assert.Equal(expected, actual.Value);
        Assert.Equal("GetComment", actual.ActionName);
        Assert.Equal(KeyValuePair.Create("ID", (object?)1), actual.RouteValues?.Single());
    }

    [Fact]
    public async Task CreateComment_Given_CommentCreateDTO_With_Invalid_ContentID_Returns_NotFound()
    {
        // Act
        var response = await _controller.CreateComment(new CommentUserDTO(-1, "Text"));

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task UpdateComment_Given_Valid_ID_Returns_NoContent()
    {
        // Act
        var response = await _controller.UpdateComment(1, new CommentUpdateDTO("Text", 42));

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateComment_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var comment = new CommentUpdateDTO("Text", -1);
        _service.Setup(m => m.UpdateComment(-1, comment)).ThrowsAsync(new CommentNotFoundException(-1));

        // Act
        var response = await _controller.UpdateComment(-1, comment);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task UpdateComment_Without_Authorization_Returns_Forbid()
    {
        // Arrange
        _auth.Setup(x => x.AuthorizeAsync(It.IsNotNull<ClaimsPrincipal>(), It.Is<object>(x => x is IAuthored), It.IsNotNull<string>()))
            .ReturnsAsync(AuthorizationResult.Failed);

        // Act
        var response = await _controller.UpdateComment(-1, new CommentUpdateDTO("Text", 1));

        // Assert
        Assert.IsType<ForbidResult>(response);
    }

    [Fact]
    public async Task DeleteComment_Given_Valid_ID_Returns_NoContent()
    {
        // Act
        var response = await _controller.DeleteComment(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task DeleteComment_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        _service.Setup(m => m.RemoveComment(-1)).ThrowsAsync(new CommentNotFoundException(-1));

        // Act
        var response = await _controller.DeleteComment(-1);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task DeleteComment_Without_Authorization_Returns_Forbid()
    {
        // Arrange
        _auth.Setup(x => x.AuthorizeAsync(It.IsNotNull<ClaimsPrincipal>(), It.Is<object>(x => x is IAuthored), It.IsNotNull<string>()))
            .ReturnsAsync(AuthorizationResult.Failed);

        // Act
        var response = await _controller.DeleteComment(1);

        // Assert
        Assert.IsType<ForbidResult>(response);
    }

    [Fact]
    public async Task UpvoteComment_Given_Valid_ID_Returns_NoContent()
    {
        // Act
        var response = await _controller.UpvoteComment(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpvoteComment_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        _service.Setup(m => m.UpvoteComment(-1)).ThrowsAsync(new CommentNotFoundException(-1));

        // Act
        var response = await _controller.UpvoteComment(-1);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task DownvoteComment_Given_Valid_ID_Returns_NoContent()
    {
        // Act
        var response = await _controller.DownvoteComment(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task DownvoteComment_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        _service.Setup(m => m.DownvoteComment(-1)).ThrowsAsync(new CommentNotFoundException(-1));

        // Act
        var response = await _controller.DownvoteComment(-1);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }
}
