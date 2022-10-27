namespace TesteRabbitMQ
{
    public interface IWorker
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}
