using CheckInService.Models;
using EventStore.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Messages.Mapper;
using System.Threading;
using System.Text.Json;
using CheckInService.CommandsAndEvents.Commands;
using RabbitMQ.Messages.Messages;
using Newtonsoft.Json;
using static EventStore.Client.StreamMessage;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CheckInService.DBContexts;
using Microsoft.EntityFrameworkCore;
using CheckInService.CommandHandlers;
using Event = RabbitMQ.Messages.Messages.Event;
using CheckInService.CommandsAndEvents.Events.CheckIn;
using CheckInService.CommandsAndEvents.Events.Appointment;
using CheckInService.CommandsAndEvents.Commands.Appointment;
using CheckInService.CommandsAndEvents.Commands.CheckIn;

namespace CheckInService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplayController : ControllerBase
    {
        private readonly EventStoreClient client;
        private readonly CheckInContextDB checkInContext;
        private readonly ReplayHandler replayEventHandler;
        private readonly CheckInCommandHandler checkInCommandHandler;

        public ReplayController(EventStoreClient client, 
            CheckInContextDB checkInContext, 
            ReplayHandler replayEventHandler,
            CheckInCommandHandler checkInCommandHandler)
        {
            this.client = client;
            this.checkInContext = checkInContext;
            this.replayEventHandler = replayEventHandler;
            this.checkInCommandHandler = checkInCommandHandler;
        }

        [HttpGet(Name = "GetAllEvents")]
        public async Task<IActionResult> GetAllEvents(CancellationToken cancellationToken)
        {
            var result = client.ReadStreamAsync(
                Direction.Forwards,
                nameof(CheckIn),
                StreamPosition.Start,
                cancellationToken: cancellationToken, resolveLinkTos: true
            );
            var events = await result.ToListAsync(cancellationToken);

            // Assuming events are in JSON format
            return Ok(events);
        }

        [HttpDelete(Name = "ClearDatabase")]
        public IActionResult ClearCheckInDatabase()
        {
            // For now only the checkin table will be cleared.
            // Deletes all elements from checkin 
            var checkIns = checkInContext.checkIns.ToList();
            var tempAppointments = checkInContext.Appointments.ToList();
            var physicians = checkInContext.Physicians.ToList();
            var patients = checkInContext.Patients.ToList();
            checkInContext.checkIns.RemoveRange(checkIns);
            checkInContext.Appointments.RemoveRange(tempAppointments);
            checkInContext.Physicians.RemoveRange(physicians);
            checkInContext.Patients.RemoveRange(patients);

            checkInContext.SaveChanges();
            // Later on, all the other tables will be cleared too.
            return Ok("All checkIns have been deleted.");
        }

        [HttpPatch(Name = "Replay")]
        public async Task<IActionResult> ReplayAll()
        {
            List<Message> list = new List<Message>();
            var result = client.ReadStreamAsync(
                Direction.Forwards,
                nameof(CheckIn),
                StreamPosition.Start,
                resolveLinkTos: true
            );
            var events = await result.ToListAsync();
            foreach (var command in events)
            {
                string EventType = command.OriginalEvent.EventType;
                byte[] data = command.OriginalEvent.Data.ToArray();
                Message? entity_event = null;
                Console.WriteLine(EventType);
                // Hier zullen wijzigingen doorgevoerd moeten worden.
                switch (EventType)
                {
                    case nameof(CheckInNoShowEvent):
                        entity_event = data.Deserialize<NoShowCheckIn>();
                        await checkInCommandHandler.ChangeToNoShow((NoShowCheckIn)entity_event);
                        break;
                    case nameof(CheckInPresentEvent):
                        entity_event = data.Deserialize<PresentCheckin>();
                        await checkInCommandHandler.ChangeToPresent((PresentCheckin)entity_event);
                        break;
                    case nameof(CheckInRegistrationEvent):
                        RegisterCheckin registerCommand = data.Deserialize<RegisterCheckin>();
                        await checkInCommandHandler.RegisterCheckin(registerCommand);
                        break;
                    case nameof(AppointmentDeleteEvent):
                        var delete_command = data.Deserialize<AppointmentDeleteCommand>();
                        await checkInCommandHandler.DeleteAppointment(delete_command);
                        break;
                    case nameof(AppointmentUpdateEvent):
                        var update_command = data.Deserialize<AppointmentDeleteCommand>();
                        await checkInCommandHandler.DeleteAppointment(update_command);
                        break;
                    default:
                        Console.WriteLine($"No object convertion possible with {EventType}");
                        break;
                }
                list.Add(entity_event);
                Console.WriteLine("============== Cycle over process ===========");
            }
            return Ok(list);
        }
    }
}
