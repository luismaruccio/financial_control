using FinancialControl.Infrastructure.Messaging.Enums;

namespace FinancialControl.Infrastructure.Messaging.Queues;

public static class QueueList
{
    public static readonly List<QueueConfig> Queues =
    [
        new QueueConfig()
        {
            Name = QueueEnum.NotificationQueue,
            Durable = true,
            Exclusive = false,
            AutoDelete = false,
        }
    ];
}
