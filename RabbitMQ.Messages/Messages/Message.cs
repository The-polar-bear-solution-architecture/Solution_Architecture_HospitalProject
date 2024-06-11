using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Messages.Messages
{
    public class Message
    {
        public readonly string MessageId;
        public readonly string MessageType;

        public Message() : this(Guid.NewGuid())
        {
        }

        public Message(Guid messageId)
        {
            MessageId = messageId.ToString();
            MessageType = this.GetType().Name;
        }

        public Message(string messageType) : this(Guid.NewGuid())
        {
            MessageType = messageType;
        }

        public Message(Guid messageId, string messageType)
        {
            MessageId = messageId.ToString();
            MessageType = messageType;
        }

    }
}
