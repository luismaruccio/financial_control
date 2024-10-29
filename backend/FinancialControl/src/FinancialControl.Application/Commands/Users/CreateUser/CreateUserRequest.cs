namespace FinancialControl.Application.Commands.Users.CreateUser;

public class CreateUserRequest(string name, string email, string password)
{
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;

    public CreateUserCommand ToCommand()
        => new(Name, Email, Password);
}
