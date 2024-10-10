using FinancialControl.Domain.Entities;

namespace FinancialControl.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> AddUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(User user);
}
