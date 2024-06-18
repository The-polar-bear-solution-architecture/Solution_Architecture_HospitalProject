using CheckinService.Model;
using CheckInService.CommandHandlers;
using CheckInService.CommandsAndEvents.Commands.Appointment;
using CheckInService.CommandsAndEvents.Commands.CheckIn;
using CheckInService.CommandsAndEvents.Events.CheckIn;
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
        private readonly EventStoreRepository eventStoreRepository;
        private readonly IPublisher publisher;
        private readonly string RouterKeyLocator;

        public CheckInController(
            CheckInRepository checkInRepository, 
            CheckInCommandHandler checkInCommand,
            EventStoreRepository eventStoreRepository,
            IPublisher publisher) {
            this.checkInRepository = checkInRepository;
            this.checkInCommand = checkInCommand;
            this.eventStoreRepository = eventStoreRepository;
            this.publisher = publisher;
            RouterKeyLocator = "Notifications";
        }

        // GET: api/<CheckInController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(checkInRepository.Get());
        }

        // GET api/<CheckInController>/5
        [HttpGet("{serialNr}")]
        public IActionResult Get(Guid serialNr)
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
        public async Task<IActionResult> PutNoShow(Guid serialNr)
        {
            NoShowCheckIn command = new NoShowCheckIn() { 
                CheckInSerialNr = serialNr, Status = Status.NOSHOW
            };
            CheckInNoShowEvent? NoShowEvent = await checkInCommand.ChangeToNoShow(command);
            if (NoShowEvent == null)
            {
                return NotFound();
            }
            // Add event to event store.
            await eventStoreRepository.StoreMessage(nameof(CheckIn), NoShowEvent.MessageType, NoShowEvent);

            return Ok("Marked appointment as noshow");
        }

        [HttpPut("{serialNr}/MarkPresent")]
        public async Task<IActionResult> PutPresentAsync(Guid serialNr)
        {
            PresentCheckin command = new PresentCheckin() { CheckInSerialNr = serialNr, Status = Status.PRESENT };
            CheckInPresentEvent? PresentEvent = await checkInCommand.ChangeToPresent(command);
            if (PresentEvent == null)
            {
                return NotFound();
            }
            // Add event to event store.
            await eventStoreRepository.StoreMessage(nameof(CheckIn), PresentEvent.MessageType, PresentEvent);

            // Send notification to physician.
            await publisher.SendMessage(PresentEvent.MessageType, PresentEvent, RouterKeyLocator);

            return Ok("Marked check-in ready");
        }

        /*
        [HttpDelete("{serialNr}")]
        public async Task<IActionResult> DeleteAppointment(Guid serialNr)
        {
            AppointmentDeleteCommand command = new AppointmentDeleteCommand() { AppointmentSerialNr = serialNr };
            
            var result = await checkInCommand.DeleteAppointment(command);

            return Ok("Appointment deleted.");
        }
        */

    }
}
