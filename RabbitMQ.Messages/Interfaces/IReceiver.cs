using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Messages.Interfaces
{
    public interface IReceiver
    {
        void Start(IMessageHandleCallback CallBack);
        void Stop();
    }
}
