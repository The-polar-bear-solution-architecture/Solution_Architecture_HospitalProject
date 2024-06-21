using EventStore.Client;
using PatientService.Domain;
using PatientService.Events.Patient;
using RabbitMQ.Messages.Mapper;
using RabbitMQ.Messages.Messages;

namespace PatientService.Repository
{
    public class EventStoreRepository
    {
        private readonly EventStoreClient eventStore;

        public EventStoreRepository(EventStoreClient eventStore)
        {
            this.eventStore = eventStore;
        }

        public async Task HandlePatientCreatedEvent(Patient patient)
        {
            var createdEvent = new PatientCreatedEvent("Create");
            createdEvent.Id = patient.Id;
            createdEvent.FirstName = patient.FirstName;
            createdEvent.LastName = patient.LastName;
            createdEvent.PhoneNumber = patient.PhoneNumber;
            createdEvent.Address = patient.Address;
            createdEvent.GeneralPractitionerEmail = patient.GeneralPractitioner.Email;
            await StoreMessage("patients", "post", createdEvent);
        }

        public async Task StoreMessage(String collection, string MessageType, Message command)
        {
            byte[] data = command.Serialize();
            var eventData = new EventData(Uuid.NewUuid(), MessageType, data);
            await eventStore.AppendToStreamAsync(collection, StreamState.Any, [eventData]);
        }
    }
}
