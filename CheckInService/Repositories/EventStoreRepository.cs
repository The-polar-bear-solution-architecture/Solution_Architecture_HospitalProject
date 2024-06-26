using CheckInService.Models;
using EventStore.Client;
using RabbitMQ.Messages.Mapper;
using RabbitMQ.Messages.Messages;
using System.Collections.Specialized;

namespace CheckInService.Repositories
{
    public class EventStoreRepository
    {
        private readonly EventStoreClient eventStore;
        private readonly string Collection;

        public EventStoreRepository(EventStoreClient eventStore)
        {
            this.eventStore = eventStore;
        }

        public async Task StoreMessage(String collection, string MessageType, Message command)
        {
            byte[] data = command.Serialize();
            var eventData = new EventData(Uuid.NewUuid(), MessageType, data);
            await eventStore.AppendToStreamAsync(collection, StreamState.Any, [eventData]);
        }

        public async Task<List<ResolvedEvent>> GetFromCollection(string collection)
        {
            var result = eventStore.ReadStreamAsync(
                Direction.Forwards,
                collection,
                StreamPosition.Start,
                resolveLinkTos: true
            );
            return await result.ToListAsync();

        }
    }
}
