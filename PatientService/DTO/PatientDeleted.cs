using RabbitMQ.Messages.Messages;

namespace PatientService.DTO
{
    public class PatientDeleted : Event
    {
        public Guid Id { get; set; }

        public PatientDeleted() { }

        public PatientDeleted(Guid messageId) : base(messageId) { }

        public PatientDeleted(string messageType) : base(messageType) { }

        public PatientDeleted(Guid messageId, string messageType) : base(messageId, messageType) { }
    }
}
