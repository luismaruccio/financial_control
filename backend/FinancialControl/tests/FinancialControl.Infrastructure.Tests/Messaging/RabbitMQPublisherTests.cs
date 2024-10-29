using FinancialControl.Infrastructure.Messaging;
using FinancialControl.Infrastructure.Messaging.Connections;
using FinancialControl.Infrastructure.Messaging.Queues;
using Moq;
using RabbitMQ.Client;

namespace FinancialControl.Infrastructure.Tests.Messaging;

public class RabbitMQPublisherTests
{
    private readonly Mock<IRabbitMQConnection> _rabbitMQConnectionMock = new();
    private readonly Mock<IModel> _channelMock = new();

    public RabbitMQPublisherTests()
    {
        _rabbitMQConnectionMock.Setup(mock => mock.CreateChannel()).Returns(_channelMock.Object);
        _channelMock.Setup(mock => mock.QueueDeclare(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>())).Returns(new QueueDeclareOk("Test", 0, 0));
    }

    [Fact]
    public void RabbitMQPublisher_ShouldInitializeQueues()
    {
        var publisher = new RabbitMQPublisher<object>(_rabbitMQConnectionMock.Object);

        foreach (var queue in QueueList.Queues)
        {
            _channelMock.Verify(mock => mock.QueueDeclare(queue.Name,
                                                          queue.Durable,
                                                          queue.Exclusive,
                                                          queue.AutoDelete,
                                                          null), Times.Once);
        }
    }
}
