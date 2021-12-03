using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SELearning.API.Controllers;
using SELearning.Core.Comment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SELearning.API.Tests;

public class CommentControllerTest
{
    [Fact]
    public async Task GetComment_Given_Valid_ID_Returns_Comment()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var service = new Mock<ICommentService>();
        var controller = new CommentController(logger.Object, service.Object);

        var expected = new Comment { Id = 1 };
        service.Setup(m => m.GetCommentFromCommentId(1)).ReturnsAsync(expected);

        // Act
        var actual = ((await controller.GetComment(1)).Result as OkObjectResult)!.Value;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetComment_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var service = new Mock<ICommentService>();
        var controller = new CommentController(logger.Object, service.Object);

        service.Setup(m => m.GetCommentFromCommentId(-1)).ThrowsAsync(new Exception());

        // Act
        var response = (await controller.GetComment(-1)).Result;

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task GetCommentsByContentID_Given_Valid_ID_Returns_Comments()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var service = new Mock<ICommentService>();
        var controller = new CommentController(logger.Object, service.Object);

        var expected = new List<Comment>();
        service.Setup(m => m.GetCommentsFromContentId(1)).ReturnsAsync(expected);

        // Act
        var actual = ((await controller.GetCommentsByContentID(1)).Result as OkObjectResult)!.Value;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetCommentsByContentID_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var service = new Mock<ICommentService>();
        var controller = new CommentController(logger.Object, service.Object);

        service.Setup(m => m.GetCommentsFromContentId(-1)).ThrowsAsync(new Exception());

        // Act
        var response = (await controller.GetCommentsByContentID(-1)).Result;

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task CreateComment_Given_CommentCreateDTO_With_Valid_ContentID_Returns_CreatedAtRoute()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var service = new Mock<ICommentService>();
        var controller = new CommentController(logger.Object, service.Object);

        var comment = new CommentCreateDTO("Author", "Text", 1);

        // Act
        var result = (await controller.CreateComment(comment) as CreatedAtRouteResult)!;

        // Assert
        Assert.Equal("GetComment", result.RouteName);
        Assert.Equal(comment.ContentId, result.Value);
    }

    [Fact]
    public async Task CreateComment_Given_CommentCreateDTO_With_Invalid_ContentID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var service = new Mock<ICommentService>();
        var controller = new CommentController(logger.Object, service.Object);

        var comment = new CommentCreateDTO("Author", "Text", -1);
        service.Setup(m => m.PostComment(comment)).ThrowsAsync(new Exception());

        // Act
        var response = await controller.CreateComment(comment);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task UpdateComment_Given_Valid_ID_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var service = new Mock<ICommentService>();
        var controller = new CommentController(logger.Object, service.Object);

        // Act
        var response = await controller.UpdateComment(1, new CommentUpdateDTO("Text", 42));

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateComment_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var service = new Mock<ICommentService>();

        var comment = new CommentUpdateDTO("Text", -1);
        service.Setup(m => m.UpdateComment(-1, comment)).ThrowsAsync(new Exception());

        var controller = new CommentController(logger.Object, service.Object);

        // Act
        var response = await controller.UpdateComment(-1, comment);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task DeleteComment_Given_Valid_ID_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var service = new Mock<ICommentService>();
        var controller = new CommentController(logger.Object, service.Object);

        // Act
        var response = await controller.DeleteComment(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task DeleteComment_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var service = new Mock<ICommentService>();
        var controller = new CommentController(logger.Object, service.Object);

        service.Setup(m => m.RemoveComment(-1)).ThrowsAsync(new Exception());

        // Act
        var response = await controller.DeleteComment(-1);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }
}
