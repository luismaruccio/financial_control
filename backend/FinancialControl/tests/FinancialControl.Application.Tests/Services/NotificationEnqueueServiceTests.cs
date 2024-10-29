using FinancialControl.Application.Services.Notifications;
using FinancialControl.Domain.DTOs.Notifications;
using FinancialControl.Domain.Interfaces.Publishers;
using FinancialControl.Infrastructure.Messaging.Enums;
using FluentAssertions;
using Moq;

namespace FinancialControl.Application.Tests.Services;

public class NotificationEnqueueServiceTests
{
    private Mock<IMessagePublisher<NotificationDTO>> _messagePublisherMock = new();
    private NotificationEnqueueService _notificationEnqueueService;

    public NotificationEnqueueServiceTests()
    {
        _notificationEnqueueService = new NotificationEnqueueService(_messagePublisherMock.Object);
    }

    [Fact]
    public async Task EnqueueNotification_ShouldPublishMessage()
    {
        var notificationDTO = new NotificationDTO()
        {
            Recipient = new Domain.Enums.Notifications.RecipientDTO()
            {
                Name = "Test",
                Email = "test@test.com"
            },
            NotificationType = Domain.Enums.Notifications.NotificationType.ValidationEmail,
            Params = []
        };

        await _notificationEnqueueService.EnqueueNotification(notificationDTO);

        _messagePublisherMock.Verify(mock => mock.PublishMessageAsync(It.IsAny<NotificationDTO>(), QueueEnum.NotificationQueue));

    }
}
