using FinancialControl.API.Controllers;
using FinancialControl.Application.Commands.Users.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FinancialControl.API.Tests.Controllers;

public class UserControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly UserController _userController;

    public UserControllerTests()
    {
        _userController = new UserController(_mediatorMock.Object);
    }

    [Fact]
    public async Task CreateUser_WhenUserCreated_ShouldReturnHttpStatusOk()
    {
        var commnadResult = new CreateUserResponse(Success: true, Message: string.Empty);

        _mediatorMock.Setup(mock => mock.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(commnadResult);

        var request = new CreateUserRequest(name: "Test", email: "test@test.com", password: "P4$$w0rd");

        var result = await _userController.CreateUser(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task CreateUser_WhenUserIsNotCreated_ShouldReturnHttpStatusBadRequest()
    {
        var commnadResult = new CreateUserResponse(Success: false, Message: string.Empty);

        _mediatorMock.Setup(mock => mock.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(commnadResult);

        var request = new CreateUserRequest(name: "Test", email: "test@test.com", password: "P4$$w0rd");

        var result = await _userController.CreateUser(request);

        var okResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, okResult.StatusCode);
    }
}
