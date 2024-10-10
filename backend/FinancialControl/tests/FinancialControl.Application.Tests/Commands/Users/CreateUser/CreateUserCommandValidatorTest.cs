using FinancialControl.Application.Commands.Users.CreateUser;
using FluentValidation.TestHelper;

namespace FinancialControl.Application.Tests.Commands.Users.CreateUser;

public class CreateUserCommandValidatorTest
{
    private readonly CreateUserCommandValidator commandValidator;

    public CreateUserCommandValidatorTest()
    {
        commandValidator = new CreateUserCommandValidator();
    }

    [Fact]
    public void Validate_WhenTheNameIsEmpty_ShouldReturnError()
    {
        var command = new CreateUserCommand(name: string.Empty, email: "test@test.com", password: "P4sw0rd");
        var result = commandValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(command => command.Name);
    }

    [Fact]
    public void Validate_WhenThenTheEmailIsEmpty_ShouldReturnError()
    {
        var command = new CreateUserCommand(name: "Test", email: string.Empty, password: "P4sw0rd");
        var result = commandValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(command => command.Email);
    }

    [Fact]
    public void Validate_WhenThenTheEmailIsInvalid_ShouldReturnError()
    {
        var command = new CreateUserCommand(name: "Test", email: "invalid_email", password: "P4sw0rd");
        var result = commandValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(command => command.Email);
    }

    [Fact]
    public void Validate_WhenThenThePasswordIsEmpty_ShouldReturnError()
    {
        var command = new CreateUserCommand(name: "Test", email: "invalid_email", password: string.Empty);
        var result = commandValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(command => command.Password);
    }

    [Theory]
    [InlineData("1234")]
    [InlineData("xhMwLJzu9Ks95T&n^#t@cQ@NwerhZvgRJGto63^Nbd9iKEyY52fqX^kFhFcZF35A8#ntbv")]
    public void Validate_WhenThenThePasswordIsInvalid_ShouldReturnError(string password)
    {
        var command = new CreateUserCommand(name: "Test", email: "invalid_email", password: password);
        var result = commandValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(command => command.Password);
    }

}

