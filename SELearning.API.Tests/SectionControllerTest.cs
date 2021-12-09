using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SELearning.API.Controllers;
using SELearning.Core;
using SELearning.Core.Content;
using SELearning.Core.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SELearning.API.Tests;

public class SectionControllerTest
{
    [Fact]
    public async Task GetContentsBySectionID_Given_Invalid_Section_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<SectionController>>();
        var service = new Mock<ISectionService>();
        var controller = new SectionController(logger.Object, service.Object);

        var expected = Array.Empty<ContentDto>();
        service.Setup(m => m.GetContentInSection(-1)).ThrowsAsync(new SectionNotFoundException(-1));

        // Act
        var response = (await controller.GetContentsBySectionID(-1)).Result;

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task GetSection_Given_Valid_ID_Returns_ContentDTO()
    {
        // Arrange
        var logger = new Mock<ILogger<SectionController>>();
        var service = new Mock<ISectionService>();
        var controller = new SectionController(logger.Object, service.Object);

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
        var logger = new Mock<ILogger<SectionController>>();
        var service = new Mock<ISectionService>();
        var controller = new SectionController(logger.Object, service.Object);

        service.Setup(m => m.GetSection(-1)).ThrowsAsync(new SectionNotFoundException(-1));

        // Act
        var response = (await controller.GetSection(-1)).Result;

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task GetAllSections_Returns_Collection_Of_Sections()
    {
        // Arrange
        var logger = new Mock<ILogger<SectionController>>();
        var service = new Mock<ISectionService>();
        var controller = new SectionController(logger.Object, service.Object);

        var expected = Array.Empty<SectionDto>();
        service.Setup(m => m.GetSections()).ReturnsAsync(expected);

        // Act
        var actual = ((await controller.GetAllSections()).Result as OkObjectResult)!.Value;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task CreateSection_Returns_CreatedAtRoute()
    {
        // Arrange
        var logger = new Mock<ILogger<SectionController>>();
        var service = new Mock<ISectionService>();
        var controller = new SectionController(logger.Object, service.Object);

        var toCreate = new SectionCreateDto { Title = "Title" };
        var expected = new SectionDto { Title = "Title", Id = 1 };
        service.Setup(m => m.AddSection(toCreate)).ReturnsAsync(expected);

        // Act
        var actual = (await controller.CreateSection(toCreate) as CreatedAtActionResult)!;

        // Assert
        Assert.Equal(expected, actual.Value);
        Assert.Equal("GetSection", actual.ActionName);
        Assert.Equal(KeyValuePair.Create("ID", (object?)1), actual.RouteValues?.Single());
    }

    [Fact]
    public async Task UpdateSection_Given_Valid_ID_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<SectionController>>();
        var service = new Mock<ISectionService>();
        var controller = new SectionController(logger.Object, service.Object);

        // Act
        var response = await controller.UpdateSection(1, new SectionUpdateDto { Title = "Title" });

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateSection_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<SectionController>>();
        var service = new Mock<ISectionService>();
        var controller = new SectionController(logger.Object, service.Object);

        var section = new SectionUpdateDto { Title = "Title" };
        service.Setup(m => m.UpdateSection(-1, section)).ThrowsAsync(new SectionNotFoundException(-1));

        // Act
        var response = await controller.UpdateSection(-1, section);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task DeleteSection_Given_Valid_ID_Returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<SectionController>>();
        var service = new Mock<ISectionService>();
        var controller = new SectionController(logger.Object, service.Object);

        // Act
        var response = await controller.DeleteSection(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task DeleteSection_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<SectionController>>();
        var service = new Mock<ISectionService>();
        var controller = new SectionController(logger.Object, service.Object);

        service.Setup(m => m.DeleteSection(-1)).ThrowsAsync(new SectionNotFoundException(-1));

        // Act
        var response = await controller.DeleteSection(-1);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }
}
