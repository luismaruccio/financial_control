namespace FinancialControl.Application.Interfaces.Services;

public interface IEncryptionService
{
    public string HashPassword(string password);
    public bool VerifyPassword(string password, string hashedPassword);
}
