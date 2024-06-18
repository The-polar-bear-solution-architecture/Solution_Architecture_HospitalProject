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
        [HttpGet(Name = "Rabbit")]
        public IEnumerable<string> Get()
        {
            string guid_string = "52c24165-be89-4001-8455-09d678cea45e";

            Console.WriteLine(guid_string);

            Guid.TryParse(guid_string, out var guid);

            // publisher.SendMessage("Yo", "Welkom wereld. Dit is een wereld", "Appointments_Checkin");
            return new string[] { guid.ToString() };
        }
    }
}
