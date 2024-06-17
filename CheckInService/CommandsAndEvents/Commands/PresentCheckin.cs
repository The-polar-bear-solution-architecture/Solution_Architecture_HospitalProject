﻿using CheckinService.Model;
using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Commands
{
    public class PresentCheckin : Command
    {
        public int CheckInId { get; init; }
        public string CheckInSerialNr { get; init; }
        public Status Status { get; init; } = Status.PRESENT;

        public PresentCheckin(Guid messageId) : base(messageId)
        {
        }

        public PresentCheckin(string messageType) : base(messageType)
        {
        }

        public PresentCheckin(Guid messageId, string messageType) : base(messageId, messageType)
        {
        }

        public PresentCheckin() : base(Guid.NewGuid(), nameof(PresentCheckin)) { }
    }
}
