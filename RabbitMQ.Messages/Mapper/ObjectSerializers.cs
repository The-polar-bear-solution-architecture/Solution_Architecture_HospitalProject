using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Messages.Mapper
{
    public static class ObjectSerializers
    {
        public static byte[] Serialize(this object objectToBeSerialized)
        {
            return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(objectToBeSerialized);
        }

        public static T Deserialize<T>(this byte[] objectToBeSerialize)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(objectToBeSerialize);
        }
    }
}
