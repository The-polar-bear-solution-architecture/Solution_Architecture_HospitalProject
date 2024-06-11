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

        public RabbitController(IPublisher publisher)
        {
            this.publisher = publisher;
        }

        // GET: api/<RabbitController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            publisher.SendMessage("Yo", "Welkom wereld", "De_Queue");
            return new string[] { "value1", "value2" };
        }
    }
}
