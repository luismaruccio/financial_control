using FinancialControl.Domain.DTOs.Notifications;

namespace FinancialControl.Application.Interfaces.Services;

public interface INotificationEnqueueService
{
    public Task EnqueueNotification(NotificationDTO notification);
}
