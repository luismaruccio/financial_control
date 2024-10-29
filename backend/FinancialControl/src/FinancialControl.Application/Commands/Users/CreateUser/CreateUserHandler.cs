using FinancialControl.Application.Commands.Notifications.EmailValidation;
using FinancialControl.Application.Extensions;
using FinancialControl.Application.Interfaces.Services;
using FinancialControl.Application.Messages;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces.Repositories;
using FluentValidation;
using MediatR;

namespace FinancialControl.Application.Commands.Users.CreateUser;

public sealed class CreateUserHandler(IUserRepository userRepository, IEncryptionService encryptionService, IValidator<CreateUserCommand> validator, IMediator mediator) : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEncryptionService _encryptionService = encryptionService;
    private readonly IValidator<CreateUserCommand> _validator = validator;
    private readonly IMediator _mediator = mediator;
    public async Task<CreateUserResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var validationResponse = await ValidateCommandAsync(command);
        if (validationResponse is not null) 
            return validationResponse;

        var userExistsResponse = await CheckUserExistsAsync(command.Email);
        if (userExistsResponse is not null)
            return userExistsResponse;

        var newUser = MapToUser(command);

        var response = await AddNewUserAsync(newUser);

        if (response.Success)
        {
            await _mediator.Publish(new EmailValidationNotification(newUser), cancellationToken);
        }

        return response;
    }

    private async Task<CreateUserResponse?> ValidateCommandAsync(CreateUserCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return new CreateUserResponse(Success: false, Message: errors);
        }

        return null;
    }

    private async Task<CreateUserResponse?> CheckUserExistsAsync(string email)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(email);
        if (existingUser != null)
            return new CreateUserResponse(Success: false, Message: ErrorMessages.EmailAlreadyInUse);

        return null;
    }

    private User MapToUser(CreateUserCommand command) =>
        new()
        {
            Name = command.Name,
            Email = command.Email,
            Password = _encryptionService.HashPassword(command.Password),
            EmailVerified = false,
        };

    private async Task<CreateUserResponse> AddNewUserAsync(User newUser)
    {
        try
        {
            var created = await _userRepository.AddUserAsync(newUser);

            if (created)
                return new CreateUserResponse(Success: true, Message: SuccessMessages.CreateWithSuccess.WithParameters("The user"));
            else
                return new CreateUserResponse(Success: false, Message: ErrorMessages.FailedToCreate.WithParameters("the user"));
        }
        catch (Exception ex)
        {

            return new CreateUserResponse(Success: false, Message: ErrorMessages.ErrorWhileSaving.WithParameters("the user", ex.Message));
        }
    }
}
