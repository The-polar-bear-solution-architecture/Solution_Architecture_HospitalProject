using CheckinService.Model;
using CheckInService.CommandHandlers;
using CheckInService.CommandsAndEvents.Commands;
using CheckInService.Mapper;
using CheckInService.Models;
using CheckInService.Models.DTO;
using CheckInService.Repositories;
using EventStore.Client;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Messages.Interfaces;
using RabbitMQ.Messages.Mapper;

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
        [HttpGet("{serialNr}")]
        public IActionResult Get(string serialNr)
        {
            var checkIn = checkInRepository.Get(serialNr);
            if(checkIn == null)
            {
                return BadRequest("CheckIn not found");
            }
            return Ok(checkIn);
        }

        // PUT api/<CheckInController>/5
        [HttpPut("{serialNr}/MarkNoShow")]
        public async Task<IActionResult> PutNoShow(string serialNr)
        {
            NoShowCheckIn command = new NoShowCheckIn() { 
                CheckInSerialNr = serialNr, Status = Status.NOSHOW
            };
            CheckIn? checkIn = await checkInCommand.ChangeToNoShow(command);
            if (checkIn == null)
            {
                return NotFound();
            }

            return Ok("Marked appointment as noshow");
        }

        [HttpPut("{serialNr}/MarkPresent")]
        public async Task<IActionResult> PutPresentAsync(string serialNr)
        {
            PresentCheckin command = new PresentCheckin() { CheckInSerialNr = serialNr, Status = Status.PRESENT };
            CheckIn? checkIn = await checkInCommand.ChangeToPresent(command);
            if (checkIn == null)
            {
                return NotFound();
            }

            return Ok("Marked check-in ready");
        }
    }
}
