using MediatR;

namespace FinancialControl.Application.Commands.Users.CreateUser;

public class CreateUserCommand(string name, string email, string password) : IRequest<CreateUserCommandResponse>
{
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}
