using CheckinService.Model;
using CheckInService.CommandHandlers;
using CheckInService.CommandsAndEvents.Commands;
using CheckInService.Mapper;
using CheckInService.Models;
using CheckInService.Repositories;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Messages.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheckInService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckInController : ControllerBase
    {
        private readonly CheckInRepository checkInRepository;
        private readonly CheckInCommandHandler checkInCommand;

        public CheckInController(
            CheckInRepository checkInRepository, 
            CheckInCommandHandler checkInCommand) {
            this.checkInRepository = checkInRepository;
            this.checkInCommand = checkInCommand;
        }

        // GET: api/<CheckInController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(checkInRepository.Get());
        }

        // GET api/<CheckInController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var checkIn = checkInRepository.Get(id);
            if(checkIn == null)
            {
                return BadRequest("CheckIn not found");
            }
            return Ok(checkIn);
        }

        // PUT api/<CheckInController>/5
        [HttpPut("{id}/MarkNoShow")]
        public IActionResult PutNoShow(int id)
        {
            var command = new NoShowCheckIn(Guid.NewGuid(), nameof(NoShowCheckIn))
            {
                CheckInId = id
            };

            CheckIn? checkIn = checkInCommand.ChangeToNoShow(command);
            if (checkIn == null)
            {
                return NotFound();
            }

            return Ok("Marked appointment as noshow");
        }

        [HttpPut("{id}/MarkPresent")]
        public async Task<IActionResult> PutPresentAsync(int id)
        {
            var command = new PresentCheckin(Guid.NewGuid(), nameof(PresentCheckin)) { CheckInId = id };
            CheckIn? checkIn = await checkInCommand.ChangeToPresent(command);
            
            if (checkIn == null)
            {
                return NotFound();
            }

            return Ok("Marked check-in ready");
        }

        [HttpPost("")]
        public IActionResult Post([FromBody] CreateCheckInCommandDTO createCheckInCommand)
        {
            CheckIn checkIn = createCheckInCommand.MapToCheckin();

            checkInRepository.Post(checkIn);
            
            return Ok("Marked check-in ready");
        }
    }
}
