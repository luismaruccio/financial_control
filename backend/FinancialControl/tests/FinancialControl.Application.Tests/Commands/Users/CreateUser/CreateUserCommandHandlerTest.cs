using FinancialControl.Application.Commands.Users.CreateUser;
using FinancialControl.Application.Extensions;
using FinancialControl.Application.Interfaces.Services;
using FinancialControl.Application.Messages;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace FinancialControl.Application.Tests.Commands.Users.CreateUser
{
    public class CreateUserCommandHandlerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IEncryptionService> _encryptionServiceMock = new();
        private readonly Mock<IValidator<CreateUserCommand>> _validatorMock = new();
        private readonly CreateUserCommandHandler _createUserCommandHandler;

        public CreateUserCommandHandlerTest()
        {
            _createUserCommandHandler = new CreateUserCommandHandler(_userRepositoryMock.Object, _encryptionServiceMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Handle_WhenIsAInvalidCommand_ShouldReturnSuccessFalse()
        {
            var command = GetUserCommand();

            var validationResult = new ValidationResult(new[] { new ValidationFailure("Name", "Name is empty") });
            _validatorMock.Setup(mock => mock.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(validationResult);

            var responseExpected = new CreateUserCommandResponse(false, "Name is empty");

            var response = await _createUserCommandHandler.Handle(command, CancellationToken.None);

            response.Should().BeEquivalentTo(responseExpected);
        }

        [Fact]
        public async Task Handle_WhenUserEmailAlreadyInUse_ShouldReturnSuccessFalse()
        {
            var command = GetUserCommand();
            var alreadyUser = new User()
            {
                Name = "Another User",
                Email = "user@test.com",
                Password = "password",
                EmailVerified = false
            };
            var validationResult = new ValidationResult();

            _validatorMock.Setup(mock => mock.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(validationResult);
            _userRepositoryMock.Setup(mock => mock.GetUserByEmailAsync(command.Email)).ReturnsAsync(alreadyUser);

            var responseExpected = new CreateUserCommandResponse(false, ErrorMessages.EmailAlreadyInUse);

            var response = await _createUserCommandHandler.Handle(command, CancellationToken.None);

            response.Should().BeEquivalentTo(responseExpected);
        }

        [Fact]
        public async Task Handle_WhenCalledWithValidCommand_ShouldCallToHassPassword()
        {
            var command = GetUserCommand();
            var validationResult = new ValidationResult();

            _validatorMock.Setup(mock => mock.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(validationResult);
            _userRepositoryMock.Setup(mock => mock.GetUserByEmailAsync(command.Email)).ReturnsAsync((User?)null);
            _userRepositoryMock.Setup(mock => mock.AddUserAsync(It.IsAny<User>())).ReturnsAsync(true);

            _ = await _createUserCommandHandler.Handle(command, CancellationToken.None);

            _encryptionServiceMock.Verify(mock => mock.HashPassword(command.Password), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenCalledWithValidCommand_ShouldCreateUserAndReturnSuccess()
        {
            var command = GetUserCommand();
            var validationResult = new ValidationResult();

            _validatorMock.Setup(mock => mock.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(validationResult);
            _userRepositoryMock.Setup(mock => mock.GetUserByEmailAsync(command.Email)).ReturnsAsync((User?)null);
            _userRepositoryMock.Setup(mock => mock.AddUserAsync(It.IsAny<User>())).ReturnsAsync(true);
            _encryptionServiceMock.Setup(mock => mock.HashPassword(command.Password)).Returns("$argon2id$v=19$m=131072,t=6,p=1$H8WcSxQFH2Ha3OelT/3f9A$awbybGomRW/bOAtJG7qXYxpdnYc/2u85Oy2EDfnx17E");

            var responseExpected = new CreateUserCommandResponse(true, SuccessMessages.CreateWithSuccess.WithParameters("The user"));

            var response = await _createUserCommandHandler.Handle(command, CancellationToken.None);

            response.Should().BeEquivalentTo(responseExpected);
        }

        [Fact]
        public async Task Handle_WhenErrorOccursDuringUserCreation_ShouldReturnFailure()
        {
            var command = GetUserCommand();
            var validationResult = new ValidationResult();

            _validatorMock.Setup(mock => mock.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(validationResult);
            _userRepositoryMock.Setup(mock => mock.GetUserByEmailAsync(command.Email)).ReturnsAsync((User?)null);
            _encryptionServiceMock.Setup(mock => mock.HashPassword(command.Password)).Returns("$argon2id$v=19$m=131072,t=6,p=1$H8WcSxQFH2Ha3OelT/3f9A$awbybGomRW/bOAtJG7qXYxpdnYc/2u85Oy2EDfnx17E");
            _userRepositoryMock.Setup(mock => mock.AddUserAsync(It.IsAny<User>())).ReturnsAsync(false);
            

            var responseExpected = new CreateUserCommandResponse(false, ErrorMessages.FailedToCreate.WithParameters("the user"));

            var response = await _createUserCommandHandler.Handle(command, CancellationToken.None);

            response.Should().BeEquivalentTo(responseExpected);
        }

        [Fact]
        public async Task Handle_WhenExceptionThrownDuringUserCreation_ShouldReturnFailure()
        {
            var command = GetUserCommand();
            var validationResult = new ValidationResult();

            _validatorMock.Setup(mock => mock.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(validationResult);
            _userRepositoryMock.Setup(mock => mock.GetUserByEmailAsync(command.Email)).ReturnsAsync((User?)null);
            _encryptionServiceMock.Setup(mock => mock.HashPassword(command.Password)).Returns("$argon2id$v=19$m=131072,t=6,p=1$H8WcSxQFH2Ha3OelT/3f9A$awbybGomRW/bOAtJG7qXYxpdnYc/2u85Oy2EDfnx17E");

            _userRepositoryMock.Setup(mock => mock.AddUserAsync(It.IsAny<User>())).ThrowsAsync(new Exception("Error"));
            
            var responseExpected = new CreateUserCommandResponse(false, ErrorMessages.ErrorWhileSaving.WithParameters("the user", "Error"));

            var response = await _createUserCommandHandler.Handle(command, CancellationToken.None);

            response.Should().BeEquivalentTo(responseExpected);
        }

        private static CreateUserCommand GetUserCommand() =>
            new("User", "user@test.com", "password@123");


    }

}
