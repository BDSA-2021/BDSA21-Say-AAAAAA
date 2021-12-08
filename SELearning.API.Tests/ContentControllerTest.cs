using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SELearning.API.Controllers;
using SELearning.Core.Content;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SELearning.API.Tests;

public class ContentControllerTest
{
    [Fact]
    public async Task GetContent_Given_Valid_ID_Returns_ContentDTO()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        var expected = new ContentDto { Id = 1 };
        service.Setup(m => m.GetContent(1)).ReturnsAsync(expected);

        // Act
        var actual = ((await controller.GetContent(1)).Result as OkObjectResult)!.Value;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetContent_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        service.Setup(m => m.GetContent(-1)).ThrowsAsync(new Exception());

        // Act
        var response = (await controller.GetContent(-1)).Result;

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task GetAllContent_Returns_Contents_If_Service_Can_Find()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        var expected = Array.Empty<ContentDto>();
        service.Setup(m => m.GetContent()).ReturnsAsync(expected);

        // Act
        var actual = ((await controller.GetAllContent()).Result as OkObjectResult)!.Value;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetAllContent_Returns_NotFound_If_Service_Can_Not_Find()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        service.Setup(m => m.GetContent()).ThrowsAsync(new Exception());

        // Act
        var response = (await controller.GetAllContent()).Result;

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task GetContentsBySectionID_Given_Valid_Section_ID_Returns_Contents()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        var expected = Array.Empty<ContentDto>();
        service.Setup(m => m.GetContentInSection(1)).ReturnsAsync(expected);

        // Act
        var actual = ((await controller.GetContentsBySectionID(1)).Result as OkObjectResult)!.Value;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetContentsBySectionID_Given_Invalid_Section_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        var expected = Array.Empty<ContentDto>();
        service.Setup(m => m.GetContentInSection(-1)).ThrowsAsync(new Exception());

        // Act
        var response = (await controller.GetContentsBySectionID(-1)).Result;

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task CreateContent_Returns_CreatedAtRoute_If_Service_Can_Create()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        var content = new ContentCreateDto { Title = "Title" };

        // Act
        var result = (await controller.CreateContent(content) as CreatedAtRouteResult)!;

        // Assert
        Assert.Equal("GetContent", result.RouteName);
        Assert.Equal(content, result.Value);
    }

    [Fact]
    public async Task CreateContent_Returns_BadRequest_If_Service_Can_Not_Create()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        var content = new ContentCreateDto { Title = "Title" };
        service.Setup(m => m.AddContent(content)).ThrowsAsync(new Exception());

        // Act
        var response = await controller.CreateContent(content);

        // Assert
        Assert.IsType<BadRequestResult>(response);
    }

    [Fact]
    public async Task UpdateContent_Given_Valid_ID_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        // Act
        var response = await controller.UpdateContent(1, new ContentUpdateDto { Title = "Title" });

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateContent_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        var content = new ContentUpdateDto { Title = "Title" };
        service.Setup(m => m.UpdateContent(-1, content)).ThrowsAsync(new Exception());

        // Act
        var response = await controller.UpdateContent(-1, content);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task DeleteContent_Given_Valid_ID_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        // Act
        var response = await controller.DeleteContent(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task DeleteContent_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        service.Setup(m => m.DeleteContent(-1)).ThrowsAsync(new Exception());

        // Act
        var response = await controller.DeleteContent(-1);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task UpvoteContent_Given_Valid_ID_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        // Act
        var response = await controller.UpvoteContent(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpvoteContent_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        service.Setup(m => m.IncreaseContentRating(-1)).ThrowsAsync(new Exception());

        // Act
        var response = await controller.UpvoteContent(-1);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task DownvoteContent_Given_Valid_ID_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        // Act
        var response = await controller.DownvoteContent(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task DownvoteContent_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        service.Setup(m => m.DecreaseContentRating(-1)).ThrowsAsync(new Exception());

        // Act
        var response = await controller.DownvoteContent(-1);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task GetSection_Given_Valid_ID_Returns_ContentDTO()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        var expected = new SectionDto { Id = 1 };
        service.Setup(m => m.GetSection(1)).ReturnsAsync(expected);

        // Act
        var actual = ((await controller.GetSection(1)).Result as OkObjectResult)!.Value;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetSection_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        service.Setup(m => m.GetSection(-1)).ThrowsAsync(new Exception());

        // Act
        var response = (await controller.GetSection(-1)).Result;

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task GetAllSections_Returns_Sections_If_Service_Can_Find()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        var expected = Array.Empty<SectionDto>();
        service.Setup(m => m.GetSections()).ReturnsAsync(expected);

        // Act
        var actual = ((await controller.GetAllSections()).Result as OkObjectResult)!.Value;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetAllSections_Returns_NotFound_If_Service_Can_Not_Find()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        service.Setup(m => m.GetSections()).ThrowsAsync(new Exception());

        // Act
        var response = (await controller.GetAllSections()).Result;

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task CreateSection_Returns_CreatedAtRoute_If_Service_Can_Create()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        var section = new SectionCreateDto { Title = "Title" };

        // Act
        var result = (await controller.CreateSection(section) as CreatedAtRouteResult)!;

        // Assert
        Assert.Equal("GetSection", result.RouteName);
        Assert.Equal(section, result.Value);
    }

    [Fact]
    public async Task CreateService_Returns_BadRequest_If_Service_Can_Not_Create()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        var section = new SectionCreateDto { Title = "Title" };
        service.Setup(m => m.AddSection(section)).ThrowsAsync(new Exception());

        // Act
        var response = await controller.CreateSection(section);

        // Assert
        Assert.IsType<BadRequestResult>(response);
    }

    [Fact]
    public async Task UpdateSection_Given_Valid_ID_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        // Act
        var response = await controller.UpdateSection(1, new SectionUpdateDto { Title = "Title" });

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateSection_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        var section = new SectionUpdateDto { Title = "Title" };
        service.Setup(m => m.UpdateSection(-1, section)).ThrowsAsync(new Exception());

        // Act
        var response = await controller.UpdateSection(-1, section);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task DeleteSection_Given_Valid_ID_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        // Act
        var response = await controller.DeleteSection(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task DeleteSection_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var service = new Mock<IContentService>();
        var controller = new ContentController(logger.Object, service.Object);

        service.Setup(m => m.DeleteSection(-1)).ThrowsAsync(new Exception());

        // Act
        var response = await controller.DeleteSection(-1);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }
}
