namespace SELearning.API.Tests;

public class SectionControllerTest
{
    private readonly SectionController _controller;
    private readonly Mock<ISectionService> _service;

    public SectionControllerTest()
    {
        var logger = new Mock<ILogger<SectionController>>();
        _service = new Mock<ISectionService>();
        _controller = new SectionController(logger.Object, _service.Object);
    }

    [Fact]
    public async Task GetContentsBySectionID_Given_Invalid_Section_ID_Returns_NotFound()
    {
        // Arrange
        var expected = Array.Empty<ContentDTO>();
        _service.Setup(m => m.GetContentInSection(-1)).ThrowsAsync(new SectionNotFoundException(-1));

        // Act
        var response = (await _controller.GetContentsBySectionID(-1)).Result;

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task GetSection_Given_Valid_ID_Returns_ContentDTO()
    {
        // Arrange
        var expected = new SectionDTO { Id = 1 };
        _service.Setup(m => m.GetSection(1)).ReturnsAsync(expected);

        // Act
        var actual = ((await _controller.GetSection(1)).Result as OkObjectResult)!.Value;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetSection_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        _service.Setup(m => m.GetSection(-1)).ThrowsAsync(new SectionNotFoundException(-1));

        // Act
        var response = (await _controller.GetSection(-1)).Result;

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task GetAllSections_Returns_Collection_Of_Sections()
    {
        // Arrange
        var expected = Array.Empty<SectionDTO>();
        _service.Setup(m => m.GetSections()).ReturnsAsync(expected);

        // Act
        var actual = ((await _controller.GetAllSections()).Result as OkObjectResult)!.Value;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task CreateSection_Returns_CreatedAtRoute()
    {
        // Arrange
        var toCreate = new SectionCreateDTO { Title = "Title" };
        var expected = new SectionDTO { Title = "Title", Id = 1 };
        _service.Setup(m => m.AddSection(toCreate)).ReturnsAsync(expected);

        // Act
        var actual = (await _controller.CreateSection(toCreate) as CreatedAtActionResult)!;

        // Assert
        Assert.Equal(expected, actual.Value);
        Assert.Equal("GetSection", actual.ActionName);
        Assert.Equal(KeyValuePair.Create("ID", (object?)1), actual.RouteValues?.Single());
    }

    [Fact]
    public async Task UpdateSection_Given_Valid_ID_Returns_NoContent()
    {
        // Act
        var response = await _controller.UpdateSection(1, new SectionUpdateDTO { Title = "Title" });

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateSection_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        var section = new SectionUpdateDTO { Title = "Title" };
        _service.Setup(m => m.UpdateSection(-1, section)).ThrowsAsync(new SectionNotFoundException(-1));

        // Act
        var response = await _controller.UpdateSection(-1, section);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task DeleteSection_Given_Valid_ID_Returns_NoContent()
    {
        // Act
        var response = await _controller.DeleteSection(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task DeleteSection_Given_Invalid_ID_Returns_NotFound()
    {
        // Arrange
        _service.Setup(m => m.DeleteSection(-1)).ThrowsAsync(new SectionNotFoundException(-1));

        // Act
        var response = await _controller.DeleteSection(-1);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }
}
