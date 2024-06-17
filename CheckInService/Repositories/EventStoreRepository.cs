using CheckInService.Models;
using EventStore.Client;
using RabbitMQ.Messages.Mapper;
using RabbitMQ.Messages.Messages;

namespace CheckInService.Repositories
{
    public class EventStoreRepository
    {
        private readonly EventStoreClient eventStore;
        private readonly string Collection;

        public EventStoreRepository(EventStoreClient eventStore)
        {
            this.eventStore = eventStore;
            Collection = nameof(CheckIn);
        }

        public async Task StoreMessage(string MessageType, Message command)
        {
            byte[] data = command.Serialize();
            var eventData = new EventData(Uuid.NewUuid(), MessageType, data);
            await eventStore.AppendToStreamAsync(Collection, StreamState.Any, [eventData]);
        }
    }
}
