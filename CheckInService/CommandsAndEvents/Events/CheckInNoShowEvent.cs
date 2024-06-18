﻿using CheckinService.Model;
using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Events
{
    public class CheckInNoShowEvent : Event
    {
        public int CheckInId { get; init; }
        public Guid CheckInSerialNr { get; init; }
        public Status Status { get; init; } = Status.NOSHOW;

        public CheckInNoShowEvent(): base(Guid.NewGuid(), nameof(CheckInPresentEvent))
        {
        }

        public CheckInNoShowEvent(Guid messageId) : base(messageId)
        {
        }

        public CheckInNoShowEvent(string messageType) : base(messageType)
        {
        }

        public CheckInNoShowEvent(Guid messageId, string messageType) : base(messageId, messageType)
        {
        }
    }
}
