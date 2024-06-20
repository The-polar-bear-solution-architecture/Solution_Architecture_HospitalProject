using Microsoft.Extensions.Configuration;
using RabbitMQ.Infrastructure.MessageHandlers;
using RabbitMQ.Infrastructure.MessagePublishers;
using RabbitMQ.Messages.Configuration;
using RabbitMQ.Messages.Interfaces;

namespace CheckInService.Configurations
{
    public interface IRabbitFactory
    {
        IPublisher CreateInternalPublisher();
        IReceiver CreateInternalReceiver();
    }

    public class InternalRabbitMQFactory: IRabbitFactory
    {
        public IConfiguration Configuration { get; }
        
        private readonly IConfiguration InternalReceiverSection;
        private readonly IConfiguration InternalPublisherSection;

        public InternalRabbitMQFactory(IConfiguration configuration) {
            Configuration = configuration;

            InternalReceiverSection = Configuration.GetSection("RabbitInternalMQHandler");
            InternalPublisherSection = Configuration.GetSection("RabbitMQInternalPublisher");
        }

        public IPublisher CreateInternalPublisher()
        {
            // Configure Internal publisher configuration
            int port = InternalPublisherSection.GetValue<int>("Port");
            string _host = InternalPublisherSection.GetValue<string>("Host");
            string _exchange = InternalPublisherSection.GetValue<string>("Exchange");

            return new RabbitMQPublisher(_host, _exchange, port, "/");
        }

        public IReceiver CreateInternalReceiver()
        {
            int port = InternalReceiverSection.GetValue<int>("Port");
            string _host = InternalReceiverSection.GetValue<string>("Host");
            string _exchange = InternalReceiverSection.GetValue<string>("Exchange");
            string queue = InternalReceiverSection.GetValue<string>("Queue");
            string customRoutingKey = InternalReceiverSection.GetValue<string>("RoutingKey");

            return new RabbitMQReceiver(_host, _exchange, queue, customRoutingKey, port, "/");
        }
    }
}
