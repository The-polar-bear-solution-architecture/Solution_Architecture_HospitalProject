using EventStore.Client;
using PatientService.Domain;
using PatientService.Events.GeneralPractitioner;
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
        public async Task HandleGPCreatedEvent(GeneralPractitioner generalPractitioner)
        {
            var gpCreatedEvent = new GPCreatedEvent("Create");
            gpCreatedEvent.Id = generalPractitioner.Id;
            gpCreatedEvent.FirstName = generalPractitioner.FirstName;
            gpCreatedEvent.LastName = generalPractitioner.LastName;
            gpCreatedEvent.PhoneNumber = generalPractitioner.PhoneNumber;
            gpCreatedEvent.Address = generalPractitioner.Address;
            gpCreatedEvent.Email = generalPractitioner.Email;
            await StoreMessage("generalPractitioners", gpCreatedEvent.MessageType, gpCreatedEvent);
        }

        public async Task HandlePatientCreatedEvent(Patient patient)
        {
            var createdEvent = new PatientCreatedEvent("Create");
            createdEvent.Id = patient.Id;
            createdEvent.FirstName = patient.FirstName;
            createdEvent.LastName = patient.LastName;
            createdEvent.PhoneNumber = patient.PhoneNumber;
            createdEvent.Address = patient.Address;
            createdEvent.GeneralPractitionerId = patient.GeneralPractitioner.Id;
            await StoreMessage("patients", createdEvent.MessageType, createdEvent);
        }

        public async Task HandlePatientUpdatedEvent(Patient patient)
        {
            var updatedEvent = new PatientUpdatedEvent("Update");
            updatedEvent.Id = patient.Id;
            updatedEvent.FirstName = patient.FirstName;
            updatedEvent.LastName = patient.LastName;
            updatedEvent.PhoneNumber = patient.PhoneNumber;
            updatedEvent.Address = patient.Address;
            updatedEvent.GeneralPractitionerId = patient.GeneralPractitioner.Id;
            await StoreMessage("patients", updatedEvent.MessageType, updatedEvent);
        }

        public async Task StoreMessage(String collection, string MessageType, Message command)
        {
            byte[] data = command.Serialize();
            var eventData = new EventData(Uuid.NewUuid(), MessageType, data);
            await eventStore.AppendToStreamAsync(collection, StreamState.Any, [eventData]);
        }
    }
}
