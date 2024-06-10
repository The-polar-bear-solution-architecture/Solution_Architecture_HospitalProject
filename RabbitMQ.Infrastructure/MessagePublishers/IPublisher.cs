﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Infrastructure.MessagePublishers
{
    public interface IPublisher
    {
        void SendMessage(string MessageType, object message, string queueKey);
    }
}
