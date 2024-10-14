using FinancialControl.Application.Services;
using System.Text.RegularExpressions;

namespace FinancialControl.Application.Tests.Services;

public partial class ValidationCodeServiceTests
{
    private readonly ValidationCodeService _validationCodeService;

    [GeneratedRegex(@"^\d{4}-\d{4}$")]
    private static partial Regex ValidationRegex();

    public ValidationCodeServiceTests()
    {
        _validationCodeService = new ValidationCodeService();
    }

    [Fact]
    public void GenerateCode_ShouldGenerateAValidationCodeAsExpected()
    {
        var regex = ValidationRegex();

        var validationCode = _validationCodeService.GenerateCode();
        Assert.Matches(regex, validationCode);
    }
}
