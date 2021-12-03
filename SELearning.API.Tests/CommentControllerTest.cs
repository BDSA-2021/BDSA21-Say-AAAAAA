using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SELearning.API.Controllers;
using SELearning.Core;
using SELearning.Core.Comment;
using SELearning.Core.Content;
using SELearning.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
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
        var repository = new Mock<ICommentRepository>();

        var expected = new Comment { Id = 1 };
        repository.Setup(m => m.GetCommentByCommentId(1)).ReturnsAsync(expected);

        var controller = new CommentController(logger.Object, (ICommentService)repository.Object);

        // Act
        var actual = (await controller.GetComment(1)).Value;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetComment_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        repository.Setup(m => m.GetCommentByCommentId(42)).ReturnsAsync(default(Comment));

        var controller = new CommentController(logger.Object, (ICommentService)repository.Object);

        // Act
        var response = await controller.GetComment(42);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async Task GetCommentsByContentID_Given_Valid_ID_Returns_Comments()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        var expected = new List<Comment>();
        repository.Setup(m => m.GetCommentsByContentId(1)).ReturnsAsync((expected, OperationResult.Succes));

        var controller = new CommentController(logger.Object, (ICommentService)repository.Object);

        // Act
        var actual = ((await controller.GetCommentsByContentID(1)).Result as OkObjectResult)!.Value;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task CreateComment_Given_CommentCreateDTO_With_Valid_ContentID_Returns_CommentDetailsDTO()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        var comment = new CommentCreateDTO("Author", "Text", 1);
        var expected = new CommentDetailsDTO("Author", "Text", 1, DateTime.Now, 42, new Content());
        repository.Setup(m => m.AddComment(comment)).ReturnsAsync((OperationResult.Created, expected));

        var controller = new CommentController(logger.Object, (ICommentService)repository.Object);

        // Act
        var result = await controller.CreateComment(1, comment) as CreatedAtRouteResult;

        // Assert
        Assert.Equal(expected, result?.Value);
        Assert.Equal("GetComment", result?.RouteName);
        Assert.Equal(KeyValuePair.Create("Id", (object?)1), result?.RouteValues?.Single());
    }

    [Fact]
    public async Task CreateComment_Given_CommentCreateDTO_With_Invalid_ContentID_ContentID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        var comment = new CommentCreateDTO("Author", "Text", 42);
        repository.Setup(m => m.AddComment(comment)).ReturnsAsync((OperationResult.NotFound, default(CommentDetailsDTO)!));

        var controller = new CommentController(logger.Object, (ICommentService)repository.Object);

        // Act
        var response = await controller.CreateComment(1, comment);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task UpdateComment_Given_Valid_ID_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        var comment = new CommentUpdateDTO("Text", 42);
        repository.Setup(m => m.UpdateComment(1, comment)).ReturnsAsync((OperationResult.Updated, default(CommentDetailsDTO)!));

        var controller = new CommentController(logger.Object, (ICommentService)repository.Object);

        // Act
        var response = await controller.UpdateComment(1, comment);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateComment_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        var comment = new CommentUpdateDTO("Text", 42);
        repository.Setup(m => m.UpdateComment(42, comment)).ReturnsAsync((OperationResult.NotFound, default(CommentDetailsDTO)!));

        var controller = new CommentController(logger.Object, (ICommentService)repository.Object);

        // Act
        var response = await controller.UpdateComment(42, comment);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task DeleteComment_Given_Valid_ID_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        repository.Setup(m => m.RemoveComment(1)).ReturnsAsync(OperationResult.Deleted);

        var controller = new CommentController(logger.Object, (ICommentService)repository.Object);

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
        var repository = new Mock<ICommentRepository>();

        repository.Setup(m => m.RemoveComment(42)).ReturnsAsync(OperationResult.NotFound);

        var controller = new CommentController(logger.Object, (ICommentService)repository.Object);

        // Act
        var response = await controller.DeleteComment(42);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }
}
