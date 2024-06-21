﻿using RabbitMQ.Messages.Messages;

namespace PatientService.Events.Patient
{
    public class PatientUpdatedEvent : Event
    {
        public PatientUpdatedEvent(string messageType) : base(messageType)
        {

        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string GeneralPractitionerEmail { get; set; }
    }
}
