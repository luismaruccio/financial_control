using FinancialControl.Application.Services;
using FluentAssertions;

namespace FinancialControl.Application.Tests.Services
{
    public class EncryptionServiceTests
    {
        private readonly EncryptionService _encryptionService;

        public EncryptionServiceTests()
        {
            _encryptionService = new EncryptionService();
        }

        [Fact]
        public void HashPassword_ShouldHashPassword()
        {
            var hashedPassword = _encryptionService.HashPassword("test");

            var verified = _encryptionService.VerifyPassword("test", hashedPassword);

            verified.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_WhenThePasswordIsIncorrect_ShouldReturnFalse()
        {
            var verified = _encryptionService.VerifyPassword("incorrect", "$argon2id$v=19$m=131072,t=6,p=1$H8WcSxQFH2Ha3OelT/3f9A$awbybGomRW/bOAtJG7qXYxpdnYc/2u85Oy2EDfnx17E");

            verified.Should().BeFalse();
        }
    }

}
