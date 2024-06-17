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
using CheckInService.CommandsAndEvents.Events;
using Event = RabbitMQ.Messages.Messages.Event;

namespace CheckInService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplayController : ControllerBase
    {
        private readonly EventStoreClient client;
        private readonly CheckInContextDB checkInContext;
        private readonly ReplayHandler checkInCommandHandler;

        public ReplayController(EventStoreClient client, CheckInContextDB checkInContext, ReplayHandler replayEventHandler)
        {
            this.client = client;
            this.checkInContext = checkInContext;
            this.checkInCommandHandler = replayEventHandler;
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
            checkInContext.checkIns.ExecuteDelete();
            // Later on, all the other tables will be cleared too.
            return Ok("All checkIns have been deleted.");
        }

        [HttpPatch(Name = "Replay")]
        public async Task<IActionResult> ReplayAll()
        {
            List<Event> list = new List<Event>();
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
                Event? checkInEvent = null;
                Console.WriteLine(EventType);
                // Hier zullen wijzigingen doorgevoerd moeten worden.
                switch (EventType)
                {
                    case nameof(CheckInNoShowEvent):
                        checkInEvent = data.Deserialize<CheckInNoShowEvent>();
                        await checkInCommandHandler.ChangeToNoShow((CheckInNoShowEvent)checkInEvent);
                        break;
                    case nameof(CheckInPresentEvent):
                        checkInEvent = data.Deserialize<CheckInPresentEvent>();
                        await checkInCommandHandler.ChangeToPresent((CheckInPresentEvent)checkInEvent);
                        break;
                    case nameof(CheckInRegistrationEvent):
                        checkInEvent = data.Deserialize<CheckInRegistrationEvent>();
                        await checkInCommandHandler.RegisterCheckin((CheckInRegistrationEvent)checkInEvent);
                        break;
                    default:
                        Console.WriteLine($"No object convertion possible with {EventType}");
                        break;
                }
                list.Add(checkInEvent);
            }
            return Ok(list);
        }

        private void PushChange(string EventType, Event checkinEvent){

        }
    }
}
