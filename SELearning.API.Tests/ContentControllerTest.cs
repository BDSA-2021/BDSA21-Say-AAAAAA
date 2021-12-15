using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SELearning.API.Controllers;
using SELearning.Core;
using SELearning.Core.Content;
using SELearning.Core.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using SELearning.Core.User;

namespace SELearning.API.Tests;

public class ContentControllerTest
{
    private readonly Mock<IContentService> _service;

    private readonly ContentController _controller;

    private readonly UserDTO _user;

    public ContentControllerTest()
    {
        var logger = new Mock<ILogger<ContentController>>();
        var authService = new Mock<IAuthorizationService>();
        _user = new UserDTO("ABC", "Joachim");
        authService.Setup(x => x.AuthorizeAsync(It.IsNotNull<ClaimsPrincipal>(), It.Is<object>(x => x is IAuthored), It.IsNotNull<string>()))
            .ReturnsAsync(AuthorizationResult.Success);

        _service = new Mock<IContentService>();
        _service.Setup(x => x.GetContent(It.Is<int>(x => x != 0)))
                .ReturnsAsync(new ContentDTO());
        _service.Setup(m => m.AddContent(It.IsNotNull<ContentCreateDto>())).ReturnsAsync(new ContentDTO { Title = "Title", Id = 1 });

        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(x => x.GetOrAddUser(It.IsNotNull<UserDTO>())).ReturnsAsync(_user);

        _controller = new ContentController(logger.Object, _service.Object, userRepo.Object, authService.Object);
        _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() };
    }

    [Fact]
    public async Task GetContent_Given_Valid_ID_Returns_ContentDTO()
    {
        // Arrange
        var expected = new ContentDTO { Id = 1 };
        _service.Setup(m => m.GetContent(1)).ReturnsAsync(expected);

        // Act
        var actual = ((await _controller.GetContent(1)).Result as OkObjectResult)!.Value;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetContent_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        _service.Setup(m => m.GetContent(-1)).ThrowsAsync(new ContentNotFoundException(-1));

        // Act
        var response = (await _controller.GetContent(-1)).Result;

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task GetAllContent_Returns_Collection_Of_Contents()
    {
        // Arrange
        var expected = Array.Empty<ContentDTO>();
        _service.Setup(m => m.GetContent()).ReturnsAsync(expected);

        // Act
        var actual = ((await _controller.GetAllContent()).Result as OkObjectResult)!.Value;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task CreateContent_Returns_CreatedAtRoute()
    {
        // Arrange
        var toCreate = new ContentUserDTO { Title = "Title", SectionId = "1" };

        // Act
        var actual = (await _controller.CreateContent(toCreate) as CreatedAtActionResult)!;

        // Assert
        Assert.Equal(new ContentDTO { Title = "Title", Id = 1 }, actual.Value);
        Assert.Equal("GetContent", actual.ActionName);
        Assert.Equal(KeyValuePair.Create("ID", (object?)1), actual.RouteValues?.Single());
    }

    [Fact]
    public async Task UpdateContent_Given_Valid_ID_Returns_NoContent()
    {
        // Act
        var response = await _controller.UpdateContent(1, new ContentUpdateDTO { Title = "Title" });

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateContent_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var content = new ContentUpdateDTO { Title = "Title" };
        _service.Setup(m => m.UpdateContent(-1, content)).ThrowsAsync(new ContentNotFoundException(-1));

        // Act
        var response = await _controller.UpdateContent(-1, content);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task DeleteContent_Given_Valid_ID_Returns_NoContent()
    {
        // Act
        var response = await _controller.DeleteContent(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task DeleteContent_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        _service.Setup(m => m.DeleteContent(-1)).ThrowsAsync(new ContentNotFoundException(-1));

        // Act
        var response = await _controller.DeleteContent(-1);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task UpvoteContent_Given_Valid_ID_Returns_NoContent()
    {
        // Act
        var response = await _controller.UpvoteContent(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpvoteContent_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        _service.Setup(m => m.IncreaseContentRating(-1)).ThrowsAsync(new ContentNotFoundException(-1));

        // Act
        var response = await _controller.UpvoteContent(-1);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task DownvoteContent_Given_Valid_ID_Returns_NoContent()
    {
        // Act
        var response = await _controller.DownvoteContent(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task DownvoteContent_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        _service.Setup(m => m.DecreaseContentRating(-1)).ThrowsAsync(new ContentNotFoundException(-1));

        // Act
        var response = await _controller.DownvoteContent(-1);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }
}
