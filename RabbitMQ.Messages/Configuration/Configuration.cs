using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Infrastructure.MessageHandlers;
using RabbitMQ.Infrastructure.MessagePublishers;
using RabbitMQ.Messages.Interfaces;

namespace RabbitMQ.Messages.Configuration
{
    public static class Configuration
    {
        private const int DEFAULT_PORT = 5672;
        private static string DEFAULT_VIRTUAL_HOST = "/";

        public static void UseRabbitMQMessageHandler(this IServiceCollection services, IConfiguration config)
        {
            var settings = GetRabbitMQSettings(config, "RabbitMQHandler");
            services.AddTransient<IReceiver>(_ => new RabbitMQReceiver(
                settings.Host, settings.Exchange, settings.Queue, settings.RoutingKey, settings.Port, settings.VirtualHost));
        }

        public static void UseRabbitMQMessagePublisher(this IServiceCollection services, IConfiguration config)
        {
            var settings = GetRabbitMQSettings(config, "RabbitMQPublisher");
            // Makes this service available for dependency injection.
            services.AddTransient<IPublisher>(_ => new RabbitMQPublisher(
                settings.Host, settings.Exchange, settings.Port, settings.VirtualHost));
        }

        private static RabbitMQSettings GetRabbitMQSettings(IConfiguration config, string sectionName)
        {
            var settings = new RabbitMQSettings();
            var errors = new List<string>();

            var configSection = config.GetSection(sectionName);
            if (!configSection.Exists())
            {
                throw new InvalidOperationException($"Required config-section '{sectionName}' not found.");
            }

            // get configuration settings
            settings.Host = DetermineHost(configSection, errors);
            settings.VirtualHost = DetermineVirtualHost(configSection, errors);
            settings.Port = DeterminePort(configSection, errors);
            settings.Exchange = DetermineExchange(configSection, errors);

            if (sectionName == "RabbitMQHandler")
            {
                settings.Queue = DetermineQueue(configSection, errors);
                settings.RoutingKey = DetermineRoutingKey(configSection, errors);
            }

            // handle possible errors
            if (errors.Any())
            {
                var errorMessage = new StringBuilder("Invalid RabbitMQ configuration:");
                errors.ForEach(e => errorMessage.AppendLine(e));
                throw new InvalidOperationException(errorMessage.ToString());
            }

            return settings;
        }

        private static string DetermineHost(IConfigurationSection configSection, List<string> errors)
        {
            var host = configSection["Host"];
            if (string.IsNullOrEmpty(host))
            {
                errors.Add("Required config-setting 'Host' not found.");
            }
            return host;
        }

        private static int DeterminePort(IConfigurationSection configSection, List<string> errors)
        {
            string portSetting = configSection["Port"];
            if (string.IsNullOrEmpty(portSetting))
            {
                return DEFAULT_PORT;
            }
            else
            {
                if (int.TryParse(portSetting, out int result))
                {
                    return result;
                }
                else
                {
                    errors.Add("Unable to parse config-setting 'Port' into an integer.");
                    return DEFAULT_PORT;
                }
            }
        }

        private static string DetermineExchange(IConfigurationSection configSection, List<string> errors)
        {
            var exchange = configSection["Exchange"];
            if (string.IsNullOrEmpty(exchange))
            {
                errors.Add("Required config-setting 'Exchange' not found.");
            }
            return exchange;
        }

        private static string DetermineQueue(IConfigurationSection configSection, List<string> errors)
        {
            var queue = configSection["Queue"];
            if (string.IsNullOrEmpty(queue))
            {
                errors.Add("Required config-setting 'Queue' not found.");
            }
            return queue;
        }

        private static string DetermineRoutingKey(IConfigurationSection configSection, List<string> errors)
        {
            var routingKey = configSection["RoutingKey"];
            if (string.IsNullOrEmpty(routingKey))
            {
                errors.Add("Required config-setting 'RoutingKey' not found.");
            }
            return routingKey;
        }

        private static string DetermineVirtualHost(IConfigurationSection configSection, List<string> errors)
        {
            var virtualHost = configSection["VirtualHost"];
            if (string.IsNullOrEmpty(virtualHost))
            {
                return DEFAULT_VIRTUAL_HOST;
            }
            return virtualHost;
        }

        private class RabbitMQSettings
        {
            public string Host { get; set; }
            public string Exchange { get; set; }
            public string Queue { get; set; }
            public string RoutingKey { get; set; }
            public int Port { get; set; }
            public string VirtualHost { get; set; }
        }

    }
}
