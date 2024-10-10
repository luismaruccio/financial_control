using FinancialControl.Application.Interfaces.Services;
using Sodium;

namespace FinancialControl.Application.Services;

public class EncryptionService : IEncryptionService
{
    public string HashPassword(string password)
        => PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Moderate);


    public bool VerifyPassword(string password, string hashedPassword)
        => PasswordHash.ArgonHashStringVerify(hashedPassword, password);
}
