using FinancialControl.Domain.Enums;

namespace FinancialControl.Domain.Entities;

public class ValidationCode
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string Code { get; set; }    
    public ValidationCodePurpose CodePurpose { get; set; }
    public DateTime ValidateDate { get; set; }

    public User? User { get; set; }
}
