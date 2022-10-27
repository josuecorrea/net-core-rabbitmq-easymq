using EasyNetQ.AutoSubscribe;
using Newtonsoft.Json;

namespace TesteRabbitMQ
{
    public class BrokerServiceHandler : BackgroundService
    {

        private readonly ILogger<BrokerServiceHandler> _logger;
        private readonly IWorker worker;


        public BrokerServiceHandler(ILogger<BrokerServiceHandler> logger, IWorker worker)
        {
            _logger = logger;
            this.worker = worker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await worker.DoWork(stoppingToken);
        }     
    }
}
