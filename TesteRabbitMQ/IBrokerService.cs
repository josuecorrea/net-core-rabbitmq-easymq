namespace TesteRabbitMQ
{
    public interface IBrokerService
    {
        Task Publish<T>(T message, string queue, string exchange);
        Task Consume<T>(string queueName) where T : class, new();
    }
}
