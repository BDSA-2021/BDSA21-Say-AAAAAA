using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SELearning.API.Controllers;
using SELearning.Core;
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
        var repository = new Mock<IContentRepository>();

        var expected = new ContentDTO(1);
        repository.Setup(m => m.GetAsync(1)).ReturnsAsync(expected);

        var controller = new ContentController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetContent(1);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetContent_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var repository = new Mock<IContentRepository>();

        repository.Setup(m => m.GetAsync(42)).ReturnsAsync(default(ContentDTO));

        var controller = new ContentController(logger.Object, repository.Object);

        // Act
        var response = await controller.GetContent(42);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }
    
    [Fact]
    public async Task CreateContent_Given_Valid_ContentDTO_Returns_Created()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var repository = new Mock<IContentRepository>();

        var content = new ContentDTO(1);
        repository.Setup(m => m.CreateAsync(content));

        var controller = new ContentController(logger.Object, repository.Object);

        // Act
        var response = await controller.CreateContent(content);

        // Assert
        Assert.IsType<CreatedResult>(response);
    }
    
    [Fact]
    public async Task CreateContent_Given_Invalid_ContentDTO_Returns_BadRequest()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var repository = new Mock<IContentRepository>();

        var content = new ContentDTO(-1);
        repository.Setup(m => m.CreateAsync(content));

        var controller = new ContentController(logger.Object, repository.Object);

        // Act
        var response = await controller.CreateContent(content);

        // Assert
        Assert.IsType<BadRequestResult>(response);
    }
    
    [Fact]
    public async Task UpdateContent_Given_Valid_ID_And_ContentDTO_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var repository = new Mock<IContentRepository>();

        var content = new ContentDTO(1);
        repository.Setup(m => m.UpdateAsync(1, content));

        var controller = new ContentController(logger.Object, repository.Object);

        // Act
        var response = await controller.UpdateContent(1, content);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }
    
    [Fact]
    public async Task UpdateContent_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var repository = new Mock<IContentRepository>();

        var content = new ContentDTO(1);
        repository.Setup(m => m.UpdateAsync(42, content));

        var controller = new ContentController(logger.Object, repository.Object);

        // Act
        var response = await controller.UpdateContent(42, content);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }
    
    [Fact]
    public async Task UpdateContent_Given_Invalid_ContentDTO_Returns_BadRequest()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var repository = new Mock<IContentRepository>();

        var content = new ContentDTO(-1);
        repository.Setup(m => m.UpdateAsync(1, content));

        var controller = new ContentController(logger.Object, repository.Object);

        // Act
        var response = await controller.UpdateContent(1, content);

        // Assert
        Assert.IsType<BadRequestResult>(response);
    }
    
    [Fact]
    public async Task DeleteContent_Given_Valid_ID_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<ContentController>>();
        var repository = new Mock<IContentRepository>();

        repository.Setup(m => m.DeleteAsync(1));

        var controller = new ContentController(logger.Object, repository.Object);

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
        var repository = new Mock<IContentRepository>();

        repository.Setup(m => m.DeleteAsync(42));

        var controller = new ContentController(logger.Object, repository.Object);

        // Act
        var response = await controller.DeleteContent(42);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }
}
