using FinancialControl.Domain.Entities;

namespace FinancialControl.Domain.Interfaces.Repositories;

public interface IValidationCodeRepository
{
    Task<bool> IsValidationCodeExistsAsync(string code, int userId);
    Task<ValidationCode?> GetValidationCodeByCodeAndUserIdAsync(string code, int userId);
    Task<bool> AddValidationCodeAsync(ValidationCode validationCode);
    Task<bool> UpdateValidationCodeAsync(ValidationCode validationCode);
    Task<bool> DeleteValidationCodeAsync(ValidationCode validationCode);
}
