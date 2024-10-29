using FinancialControl.Application.Interfaces.Services;
using FinancialControl.Domain.DTOs.Notifications;
using FinancialControl.Domain.Interfaces.Publishers;
using FinancialControl.Infrastructure.Messaging.Enums;

namespace FinancialControl.Application.Services.Notifications;

public class NotificationEnqueueService(IMessagePublisher<NotificationDTO> messagePublisher) : INotificationEnqueueService
{
    private readonly IMessagePublisher<NotificationDTO> _messagePublisher = messagePublisher;

    public async Task EnqueueNotification(NotificationDTO notification)
    {
        await _messagePublisher.PublishMessageAsync(notification, QueueEnum.NotificationQueue);
    }
}
