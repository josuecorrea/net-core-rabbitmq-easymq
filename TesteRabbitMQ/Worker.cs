using Newtonsoft.Json;

namespace TesteRabbitMQ
{
    public class Worker : IWorker
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBrokerService _brokerService;

        private int number = 0;

        public Worker(ILogger<Worker> logger, IBrokerService brokerService)
        {
            _logger = logger;
            _brokerService = brokerService;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _brokerService.Consume<BrokerClassTest>("TESTE_RABBIT_QUEUE");
                
                await Task.Delay(5000 * 5);
            }
        }
    }
}
