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
        repository.Setup(m => m.ReadAsync(1)).ReturnsAsync(expected);

        var controller = new ContentController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetContent(1);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetContent_Given_Invalid_ID_Returns_NotFound()
    {
        Assert.True(false);
    }
    
    [Fact]
    public async Task CreateContent_Given_Valid_ContentDTO_Creates_Content()
    {
        Assert.True(false);
    }
    
    [Fact]
    public async Task CreateContent_Given_Invalid_ContentDTO_Returns_BadRequest()
    {
        Assert.True(false);
    }
    
    [Fact]
    public async Task UpdateContent_Given_Valid_ID_And_ContentDTO_Updates_Content()
    {
        Assert.True(false);
    }
    
    [Fact]
    public async Task UpdateContent_Given_Invalid_ID_Returns_NotFound()
    {
        Assert.True(false);
    }
    
    [Fact]
    public async Task UpdateContent_Given_Invalid_ContentDTO_Returns_BadRequest()
    {
        Assert.True(false);
    }
    
    [Fact]
    public async Task DeleteContent_Given_Valid_ID_Deletes_Content()
    {
        Assert.True(false);
    }
    
    [Fact]
    public async Task DeleteContent_Given_Invalid_ID_Returns_NotFound()
    {
        Assert.True(false);
    }
}
