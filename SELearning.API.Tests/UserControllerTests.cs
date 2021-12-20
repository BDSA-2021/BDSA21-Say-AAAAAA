using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SELearning.API.Controllers;
using SELearning.Core.Credibility;
using SELearning.Core.User;
using Xunit;

namespace SELearning.API.Tests;

public class UserControllerTests
{
    private readonly UserController _controller;
    private readonly Mock<IUserRepository> _repository;
    private readonly Mock<ICredibilityService> _credibilityService;
    private readonly UserDTO _user = new UserDTO("ABC", "The cheese man");

    public UserControllerTests()
    {
        var logger = new Mock<ILogger<UserController>>();
        _repository = new Mock<IUserRepository>();

        var identity = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.NameIdentifier, _user.Id),
            new(ClaimTypes.GivenName, _user.Name)
        }));

        _credibilityService = new Mock<ICredibilityService>();
        _credibilityService.Setup(c => c.GetCredibilityScore(identity)).ReturnsAsync(10);

        _controller = new UserController(logger.Object, _repository.Object, _credibilityService.Object);
        _controller.ControllerContext = new ControllerContext();
        _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = identity };
    }

    [Fact]
    public async void GetCurrentUser_returns_current_user()
    {
        _repository.Setup(r => r.GetOrAddUser(_user)).ReturnsAsync(_user);
        var result = await _controller.GetCurrentUser();

        var user = (result.Result as OkObjectResult)!.Value;

        Assert.Equal(_user, user);
    }

    [Fact]
    public async void GetCurrentUserCredibility_returns_credibility_score()
    {
        var result = await _controller.GetCurrentUserCredibility();

        var score = (result.Result as OkObjectResult)!.Value;

        Assert.Equal(new UserCredibiityDTO(10), score);
    }
}
