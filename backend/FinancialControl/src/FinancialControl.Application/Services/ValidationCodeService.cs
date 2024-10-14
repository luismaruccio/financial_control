using FinancialControl.Application.Interfaces.Services;
using System.Text;

namespace FinancialControl.Application.Services
{
    public class ValidationCodeService : IValidationCodeService
    {
        private static readonly Random _random = new();
        public string GenerateCode()
        {
            var sb = new StringBuilder(9);

            for (int i = 0; i < 4; i++)
            {
                sb.Append(_random.Next(0, 10));
            }

            sb.Append('-');

            for (int i = 0; i < 4; i++)
            {
                sb.Append(_random.Next(0, 10));
            }

            return sb.ToString();
        }
    }
}
