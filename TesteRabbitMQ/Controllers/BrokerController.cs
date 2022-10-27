using Microsoft.AspNetCore.Mvc;

namespace TesteRabbitMQ.Controllers
{
    [ApiController]
    [Route("api/v1/broker")]
    public class BrokerController : ControllerBase
    {
       
        private readonly ILogger<BrokerController> _logger;
        private readonly IBrokerService _brokerService;

        public BrokerController(ILogger<BrokerController> logger, IBrokerService brokerService)
        {
            _logger = logger;
            _brokerService = brokerService;
        }

        [HttpPost]
        public async Task<IActionResult> Publish()
        {
            await _brokerService.Publish(new BrokerClassTest
            {
                CreateAt = DateTime.Now,
                Id = Guid.NewGuid(),
                Name = $"Teste - {Guid.NewGuid()} "
            }, "TESTE_RABBIT_QUEUE", "TESTE_RABBIT_EXCHANGE");

            return Ok();
        }
    }
}