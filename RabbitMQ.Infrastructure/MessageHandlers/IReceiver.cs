using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Infrastructure.MessageHandlers
{
    public interface IReceiver
    {
        void Start();
        void Stop();

        void StartReceiving();
    }
}
