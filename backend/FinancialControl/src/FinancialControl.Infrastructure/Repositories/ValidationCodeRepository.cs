using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces.Repositories;
using FinancialControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infrastructure.Repositories;

public class ValidationCodeRepository(FinancialControlDbContext context) : IValidationCodeRepository
{
    private readonly FinancialControlDbContext _context = context;

    public async Task<bool> AddValidationCodeAsync(ValidationCode validationCode)
    {
        await _context.Set<ValidationCode>().AddAsync(validationCode);
        var affectedRows = await _context.SaveChangesAsync();

        return affectedRows > 0;
    }

    public async Task<bool> DeleteValidationCodeAsync(ValidationCode validationCode)
    {
        _context.Set<ValidationCode>().Remove(validationCode);
        var affectedRows = await _context.SaveChangesAsync();

        return affectedRows > 0;
    }

    public async Task<ValidationCode?> GetValidationCodeByCodeAndUserIdAsync(string code, int userId) =>
        await _context.Set<ValidationCode>().FirstOrDefaultAsync(vc => vc.Code == code && vc.UserId == userId);

    public async Task<bool> IsValidationCodeExistsAsync(string code, int userId)
    {
        var result = await _context.Set<ValidationCode>().FirstOrDefaultAsync(vc => vc.Code == code && vc.UserId == userId);
        return result is not null;
    }

    public async Task<bool> UpdateValidationCodeAsync(ValidationCode validationCode)
    {
        _context.Set<ValidationCode>().Update(validationCode);
        var affectedRows = await _context.SaveChangesAsync();

        return affectedRows > 0;
    }
}
