using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Messages.Interfaces
{
    public interface IMessageHandleCallback
    {
        Task<bool> HandleMessageAsync(string messageType, object message);
    }
}
