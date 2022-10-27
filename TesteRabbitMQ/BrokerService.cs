using EasyNetQ;
using EasyNetQ.Topology;
using Newtonsoft.Json;
using System.Text;

namespace TesteRabbitMQ
{
    public class BrokerService : IBrokerService
    {
        private readonly IAdvancedBus _bus;
        private readonly IConfiguration _configuration;
        private ILogger<BrokerService> _logger;

        public BrokerService(ILogger<BrokerService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _bus = RabbitHutch.CreateBus("host=localhost;publisherConfirms=true;timeout=10").Advanced;
        }

        public async Task Publish<T>(T message, string queue, string exchange)
        {
            try
            {
                await Bind(await QueueDeclare(queue), await ExchangeDeclare(exchange));

                await _bus.PublishAsync(await ExchangeDeclare(exchange), "A.*", true, await Message(message))
                     .ContinueWith(task =>
                     {
                         if (task.IsCompleted)
                         {

                         }
                         if (task.IsFaulted)
                         {

                         }
                     });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error ocurred while rabbitmq message publish! ERROR -> {ex}");
            }
        }

        public async Task Consume<T>(string queueName) where T : class, new()
        {
            var queue = await _bus.QueueDeclareAsync(queueName);
            _bus.Consume<T>(queue, (msg, info) =>
               {
                   TesteRecebeMSG<T>(msg.Body);
               });

        }

        private void TesteRecebeMSG<T>(T parameter)
        {
            var x = parameter;

            _logger.LogInformation($"Passou aqui {JsonConvert.SerializeObject(x)}");
        }

        private async Task<Exchange> ExchangeDeclare(string exchangeName) => await _bus.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic);

        private async Task<Queue> QueueDeclare(string queueName) => await _bus.QueueDeclareAsync(queueName);

        private async Task Bind(Queue queue, Exchange exchange) => await _bus.BindAsync(exchange, queue, "A.*");

        private async Task<IMessage> Message<T>(T message) => new Message<T>(message);

    }
}
