using FinancialControl.Domain.Interfaces.Publishers;
using FinancialControl.Infrastructure.Messaging.Connections;
using FinancialControl.Infrastructure.Messaging.Queues;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FinancialControl.Infrastructure.Messaging;

public class RabbitMQPublisher<T> : IMessagePublisher<T>
{
    private readonly IModel _channel;

    public RabbitMQPublisher(IRabbitMQConnection connection)
    {
        _channel = connection.CreateChannel();
        InitializeQueues();
    }

    public async Task PublishMessageAsync(T message, string queueName)
    {
        var messageJson = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageJson);

        await Task.Run(() => _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body));
    }

    private void InitializeQueues()
    {
        var queues = QueueList.Queues;

        foreach (var queue in queues)
        {
            _channel.QueueDeclare(queue: queue.Name, durable: queue.Durable, exclusive: queue.Exclusive, autoDelete: queue.AutoDelete, arguments: null);
        }
    }
}
