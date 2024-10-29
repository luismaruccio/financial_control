namespace FinancialControl.Domain.Interfaces.Publishers;

public interface IMessagePublisher<T>
{
    Task PublishMessageAsync(T message, string queueName);
}
