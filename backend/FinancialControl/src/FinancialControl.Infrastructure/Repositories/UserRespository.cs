using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces.Repositories;
using FinancialControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infrastructure.Repositories;

public class UserRespository(FinancialControlDbContext context) : IUserRepository
{
    protected readonly FinancialControlDbContext _context = context;

    public async Task<bool> AddUserAsync(User user)
    {
        await _context.Set<User>().AddAsync(user);
        var affectedRows = await _context.SaveChangesAsync();

        return affectedRows > 0;

    }

    public async Task<User?> GetUserByIdAsync(int id) 
        => await _context.Set<User>().FindAsync(id);

    public async Task<User?> GetUserByEmailAsync(string email)
        => await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);

    public async Task<bool> UpdateUserAsync(User user)
    {
        _context.Set<User>().Update(user);
        var affectedRows = await _context.SaveChangesAsync();

        return affectedRows > 0;
        
    }

    public async Task<bool> DeleteUserAsync(User user)
    {
        _context.Set<User>().Remove(user);
        var affectedRows = await _context.SaveChangesAsync();

        return affectedRows > 0;

    }

}
