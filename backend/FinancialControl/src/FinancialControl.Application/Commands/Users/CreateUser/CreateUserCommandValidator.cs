using FinancialControl.Application.Extensions;
using FinancialControl.Application.Messages;
using FluentValidation;

namespace FinancialControl.Application.Commands.Users.CreateUser;
public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator() 
    {
        RuleFor(command => command.Name)
          .NotEmpty()
          .WithMessage(ValidationMessages.RequiredField.WithParameters("user name"));

        RuleFor(command => command.Email)
            .NotEmpty()
            .WithMessage(ValidationMessages.RequiredField.WithParameters("user email"))
            .EmailAddress()
            .WithMessage(ValidationMessages.InvalidField.WithParameters("user email"));

        RuleFor(command => command.Password)
            .NotEmpty()
            .WithMessage(ValidationMessages.RequiredField.WithParameters("user password"))
            .Length(8, 64)
            .WithMessage(ValidationMessages.InvalidLength.WithParameters("user password", 8, 64));
    }
}
