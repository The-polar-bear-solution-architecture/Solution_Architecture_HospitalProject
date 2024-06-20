using CheckInService.Configurations;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Messages.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// Ter voorbeeld.
namespace CheckInService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitController : ControllerBase
    {
        private readonly IPublisher publisher;

        public RabbitController(IRabbitFactory rabbitFactory)
        {
            this.publisher = rabbitFactory.CreateInternalPublisher();
        }

        [HttpPut ("TestInternalQueue")]
        public async Task<IActionResult> TestConnection()
        {
            await publisher.SendMessage("Test", "Hallo ETL", "ETL_Checkin");
            return Ok("Ok connection");
        }
    }
}
