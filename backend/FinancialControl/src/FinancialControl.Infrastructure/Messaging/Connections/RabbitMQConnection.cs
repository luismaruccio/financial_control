using RabbitMQ.Client;

namespace FinancialControl.Infrastructure.Messaging.Connections;

public class RabbitMQConnection : IRabbitMQConnection
{
    private readonly IConnection _connection;

    public RabbitMQConnection(string? uri)
    {
        ArgumentNullException.ThrowIfNull(uri);

        var factory = new ConnectionFactory() { Uri = new Uri(uri) };
        _connection = factory.CreateConnection();
    }

    public IModel CreateChannel() => _connection.CreateModel();
}
