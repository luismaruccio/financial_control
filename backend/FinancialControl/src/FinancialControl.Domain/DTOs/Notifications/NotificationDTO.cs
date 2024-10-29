using FinancialControl.Domain.Enums.Notifications;

namespace FinancialControl.Domain.DTOs.Notifications;

public class NotificationDTO
{
    public required RecipientDTO Recipient { get; set; }
    public NotificationType NotificationType { get; set; }
    public required Dictionary<string,object> Params { get; set; }
}
