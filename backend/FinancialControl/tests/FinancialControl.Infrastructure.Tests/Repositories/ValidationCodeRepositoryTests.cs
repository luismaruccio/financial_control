using FinancialControl.Domain.Entities;
using FinancialControl.Infrastructure.Data;
using FinancialControl.Infrastructure.Repositories;
using FinancialControl.Infrastructure.Tests.InMemoryDB;
using FluentAssertions;

namespace FinancialControl.Infrastructure.Tests.Repositories
{
    public class ValidationCodeRepositoryTests
    {
        private readonly FinancialControlDbContext _context;
        private readonly ValidationCodeRepository _repository;
        private readonly int _userId;
        private readonly ValidationCode _validationCode;

        public ValidationCodeRepositoryTests()
        {
            var contextFactory = new InMemoryDBContextFactory();
            _context = contextFactory.GetContext();
            _repository = new ValidationCodeRepository(_context);

            _userId = InsertUser();
            _validationCode = new ValidationCode() { UserId = _userId, Code = "1234-5678", CodePurpose = Domain.Enums.ValidationCodePurpose.ValidationEmail, ValidateDate = DateTime.Today.AddHours(5) };
        }

        [Fact]
        public async Task AddValidationCodeAsync_WhenValidationCodeIsValid_ShouldInsertTheValidationCode()
        {
            var affectedRows = await _repository.AddValidationCodeAsync(_validationCode);
            affectedRows.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteValidationCodeAsync_WhenValidationCodeIsValid_ShouldDeleteTheUser()
        {
            await _repository.AddValidationCodeAsync(_validationCode);

            var affectedRows = await _repository.DeleteValidationCodeAsync(_validationCode);

            affectedRows.Should().BeTrue();
        }

        [Fact]
        public async Task GetValidationCodeByCodeAndUserIdAsync_WhenValidationCodeExists_ShouldReturnTheValidationCode()
        {
            await _repository.AddValidationCodeAsync(_validationCode);

            var returnedValidatioCode = await _repository.GetValidationCodeByCodeAndUserIdAsync(_validationCode.Code, _userId);

            returnedValidatioCode.Should().NotBeNull();
        }

        [Fact]
        public async Task GetValidationCodeByCodeAndUserIdAsync_WhenValidationCodeNotExists_ShouldReturnNull()
        {
            await _repository.AddValidationCodeAsync(_validationCode);

            var returnedValidatioCode = await _repository.GetValidationCodeByCodeAndUserIdAsync("8765-7654", _userId);

            returnedValidatioCode.Should().BeNull();
        }

        [Theory]
        [InlineData("1234-5678", true)]
        [InlineData("6754-6543", false)]
        public async Task IsValidationCodeExistsAsync_ShouldReturnAsExpected(string validationCode, bool expected)
        {
            await _repository.AddValidationCodeAsync(_validationCode);

            var alreadyExists = await _repository.IsValidationCodeExistsAsync(validationCode, _userId);

            alreadyExists.Should().Be(expected);
        }

        [Fact]
        public async Task UpdateValidationCodeAsync_WhenValidationCodeIsValid_ShouldUpdateTheValidationCode()
        {
            await _repository.AddValidationCodeAsync(_validationCode);

            _validationCode.Code = "2345-6789";
            var affectedRows = await _repository.UpdateValidationCodeAsync(_validationCode);

            affectedRows.Should().BeTrue();
        }

        private int InsertUser()
        {
            var user = new User() { Email = "Test", Name = "Test", Password = "Test" };
            _context.Set<User>().Add(user);
            _context.SaveChanges();
            return user.Id;
        }


    }
}
