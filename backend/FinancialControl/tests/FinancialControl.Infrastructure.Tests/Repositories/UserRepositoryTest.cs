using FinancialControl.Domain.Entities;
using FinancialControl.Infrastructure.Data;
using FinancialControl.Infrastructure.Repositories;
using FinancialControl.Infrastructure.Tests.InMemoryDB;
using FluentAssertions;

namespace FinancialControl.Infrastructure.Tests.Repositories;

public class UserRepositoryTest
{
    private readonly FinancialControlDbContext _context;
    private readonly UserRespository _repository;
    private readonly User user;

    public UserRepositoryTest()
    {
        var contextFactory = new InMemoryDBContextFactory();
        _context = contextFactory.GetContext();
        _repository = new UserRespository(_context);

        user = new User() { Email = "Test", Name = "Test", Password = "Test" };
    }

    [Fact]
    public async Task AddUserAsync_WhenUserIsValid_ShouldInsertTheUser()
    {
        var affectedRows = await _repository.AddUserAsync(user);

        affectedRows.Should().Be(true);
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserExists_ShouldReturnTheUser()
    {
        await _repository.AddUserAsync(user);

        var resultedUser = await _repository.GetUserByIdAsync(user.Id);

        resultedUser.Should().NotBeNull();
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserNotExists_ShouldReturnNull()
    {
        var resultedUser = await _repository.GetUserByIdAsync(1000);

        resultedUser.Should().BeNull();
    }

    [Fact]
    public async Task GetUserByEmailAsync_WhenUserExists_ShouldReturnTheUser()
    {
        await _repository.AddUserAsync(user);

        var resultedUser = await _repository.GetUserByEmailAsync(user.Email);

        resultedUser.Should().NotBeNull();
    }

    [Fact]
    public async Task GetUserByEmailAsync_WhenUserNotExists_ShouldReturnNull()
    {
        var resultedUser = await _repository.GetUserByEmailAsync("AnotherEmail");

        resultedUser.Should().BeNull();
    }

    [Fact]
    public async Task UpdateUserAsync_WhenUserIsValid_ShouldUpdateTheUser()
    {
        await _repository.AddUserAsync(user);

        user.Name = "UpdatedUser";
        var affectedRows = await _repository.UpdateUserAsync(user);

        affectedRows.Should().Be(true);
    }

    [Fact]
    public async Task DeleteUserAsync_WhenUserIsValid_ShouldUpdateTheUser()
    {
        await _repository.AddUserAsync(user);

        var affectedRows = await _repository.DeleteUserAsync(user);

        affectedRows.Should().Be(true);
    }
}
