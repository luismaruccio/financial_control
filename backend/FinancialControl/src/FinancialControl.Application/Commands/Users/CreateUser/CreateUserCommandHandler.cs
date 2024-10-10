using FinancialControl.Application.Extensions;
using FinancialControl.Application.Interfaces.Services;
using FinancialControl.Application.Messages;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces.Repositories;
using FluentValidation;
using MediatR;

namespace FinancialControl.Application.Commands.Users.CreateUser;

public sealed class CreateUserCommandHandler(IUserRepository userRepository, IEncryptionService encryptionService, IValidator<CreateUserCommand> validator) : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEncryptionService _encryptionService = encryptionService;
    private readonly IValidator<CreateUserCommand> _validator = validator;
    public async Task<CreateUserCommandResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var validationResponse = await ValidateCommandAsync(command);
        if (validationResponse is not null) 
            return validationResponse;

        var userExistsResponse = await CheckUserExistsAsync(command.Email);
        if (userExistsResponse is not null)
            return userExistsResponse;

        var newUser = MapToUser(command);

        return await AddNewUserAsync(newUser);    
    }

    private async Task<CreateUserCommandResponse?> ValidateCommandAsync(CreateUserCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return new CreateUserCommandResponse(Success: false, Message: errors);
        }

        return null;
    }

    private async Task<CreateUserCommandResponse?> CheckUserExistsAsync(string email)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(email);
        if (existingUser != null)
            return new CreateUserCommandResponse(Success: false, Message: ErrorMessages.EmailAlreadyInUse);

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

    private async Task<CreateUserCommandResponse> AddNewUserAsync(User newUser)
    {
        try
        {
            var created = await _userRepository.AddUserAsync(newUser);

            if (created)
                return new CreateUserCommandResponse(Success: true, Message: SuccessMessages.CreateWithSuccess.WithParameters("The user"));
            else
                return new CreateUserCommandResponse(Success: false, Message: ErrorMessages.FailedToCreate.WithParameters("the user"));
        }
        catch (Exception ex)
        {

            return new CreateUserCommandResponse(Success: false, Message: ErrorMessages.ErrorWhileSaving.WithParameters("the user", ex.Message));
        }
    }
}
