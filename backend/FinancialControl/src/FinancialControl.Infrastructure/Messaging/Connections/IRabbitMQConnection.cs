using RabbitMQ.Client;

namespace FinancialControl.Infrastructure.Messaging.Connections;

public interface IRabbitMQConnection
{
    public IModel CreateChannel();
}
