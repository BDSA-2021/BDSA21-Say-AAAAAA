using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SELearning.API.Controllers;
using SELearning.Core;
using System.Threading.Tasks;
using Xunit;

namespace SELearning.API.Tests;

public class CommentControllerTest
{
    [Fact]
    public async Task GetComment_Given_Valid_ID_Returns_CommentDTO()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        var expected = new CommentDTO(1);
        repository.Setup(m => m.GetAsync(1)).ReturnsAsync(expected);

        var controller = new CommentController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetComment(1);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetComment_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        repository.Setup(m => m.GetAsync(42)).ReturnsAsync(default(CommentDTO));

        var controller = new CommentController(logger.Object, repository.Object);

        // Act
        var response = await controller.GetComment(42);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async Task CreateComment_Given_Valid_CommentDTO_Returns_Created()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        var comment = new CommentDTO(1);
        repository.Setup(m => m.CreateAsync(comment));

        var controller = new CommentController(logger.Object, repository.Object);

        // Act
        var response = await controller.CreateComment(comment);

        // Assert
        Assert.IsType<CreatedResult>(response);
    }

    [Fact]
    public async Task CreateComment_Given_Invalid_CommentDTO_Returns_BadRequest()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        var comment = new CommentDTO(-1);
        repository.Setup(m => m.CreateAsync(comment));

        var controller = new CommentController(logger.Object, repository.Object);

        // Act
        var response = await controller.CreateComment(comment);

        // Assert
        Assert.IsType<BadRequestResult>(response);
    }

    [Fact]
    public async Task UpdateComment_Given_Valid_ID_And_CommentDTO_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        var comment = new CommentDTO(1);
        repository.Setup(m => m.UpdateAsync(1, comment));

        var controller = new CommentController(logger.Object, repository.Object);

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

        var comment = new CommentDTO(1);
        repository.Setup(m => m.UpdateAsync(42, comment));

        var controller = new CommentController(logger.Object, repository.Object);

        // Act
        var response = await controller.UpdateComment(42, comment);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task UpdateComment_Given_Invalid_CommentDTO_Returns_BadRequest()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        var comment = new CommentDTO(-1);
        repository.Setup(m => m.UpdateAsync(1, comment));

        var controller = new CommentController(logger.Object, repository.Object);

        // Act
        var response = await controller.UpdateComment(1, comment);

        // Assert
        Assert.IsType<BadRequestResult>(response);
    }

    [Fact]
    public async Task DeleteComment_Given_Valid_ID_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        repository.Setup(m => m.DeleteAsync(1));

        var controller = new CommentController(logger.Object, repository.Object);

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

        repository.Setup(m => m.DeleteAsync(42));

        var controller = new CommentController(logger.Object, repository.Object);

        // Act
        var response = await controller.DeleteComment(42);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }
}
