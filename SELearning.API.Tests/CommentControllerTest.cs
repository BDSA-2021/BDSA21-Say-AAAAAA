﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SELearning.API.Controllers;
using SELearning.Core;
using System;
using System.Collections.Generic;
using System.Linq;
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

        repository.Setup(m => m.GetAsync(42)).ReturnsAsync(default(CommentDTO));

        var controller = new CommentController(logger.Object, repository.Object);

        // Act
        var response = await controller.GetComment(42);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async Task GetCommentsByContentID_Given_Valid_ID_Returns_CommentDTOs()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        var expected = Array.Empty<CommentDTO>();
        repository.Setup(m => m.GetAsyncByContentID(1)).ReturnsAsync(expected);

        var controller = new CommentController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetCommentsByContentID(1);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task CreateComment_Given_Valid_ContentID_Returns_CommentDTO()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        var comment = new CommentDTO(1);
        repository.Setup(m => m.CreateAsync(1, comment)).ReturnsAsync((OperationResult.Created, comment));

        var controller = new CommentController(logger.Object, repository.Object);

        // Act
        var result = await controller.CreateComment(1, comment) as CreatedAtRouteResult;

        // Assert
        Assert.Equal(comment, result?.Value);
        Assert.Equal("GetComment", result?.RouteName);
        Assert.Equal(KeyValuePair.Create("ID", (object?)1), result?.RouteValues?.Single());
    }

    [Fact]
    public async Task UpdateComment_Given_Valid_ID_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<CommentController>>();
        var repository = new Mock<ICommentRepository>();

        var comment = new CommentDTO(1);
        repository.Setup(m => m.UpdateAsync(1, comment)).ReturnsAsync((OperationResult.Updated, comment));

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
        repository.Setup(m => m.UpdateAsync(42, comment)).ReturnsAsync((OperationResult.NotFound, new CommentDTO(-1)));

        var controller = new CommentController(logger.Object, repository.Object);

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

        repository.Setup(m => m.DeleteAsync(1)).ReturnsAsync(OperationResult.Deleted);

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

        repository.Setup(m => m.DeleteAsync(42)).ReturnsAsync(OperationResult.NotFound);

        var controller = new CommentController(logger.Object, repository.Object);

        // Act
        var response = await controller.DeleteComment(42);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }
}