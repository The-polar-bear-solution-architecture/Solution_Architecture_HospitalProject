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

        private static string _host;
        private static string _exchange;
        private static string _queue;
        private static string _routingKey;
        private static int _port;
        private static string _virtualHost;

        private static List<string> _errors;
        private static bool _isValid;

        public static void UseRabbitMQMessageHandler(this IServiceCollection services, IConfiguration config)
        {
            GetRabbitMQSettings(config, "RabbitMQHandler");
            services.AddTransient<IReceiver>(_ => new RabbitMQReceiver(_host, _exchange, _queue, _routingKey, _port, _virtualHost));
        }

        public static void UseRabbitMQMessagePublisher(this IServiceCollection services, IConfiguration config)
        {
            GetRabbitMQSettings(config, "RabbitMQPublisher");
            // Makes this service available for dependency injection.
            services.AddTransient<IPublisher>(_ => new RabbitMQPublisher(_exchange));
        }

        private static void GetRabbitMQSettings(IConfiguration config, string sectionName)
        {
            _isValid = true;
            _errors = new List<string>();

            var configSection = config.GetSection(sectionName);
            if (!configSection.Exists())
            {
                throw new InvalidOperationException($"Required config-section '{sectionName}' not found.");
            }

            // get configuration settings
            DetermineHost(configSection);
            DetermineVirtualHost(configSection);
            DeterminePort(configSection);
            DetermineExchange(configSection);

            if (sectionName == "RabbitMQHandler")
            {
                DetermineQueue(configSection);
                DetermineRoutingKey(configSection);
            }

            // handle possible errors
            if (!_isValid)
            {
                var errorMessage = new StringBuilder("Invalid RabbitMQ configuration:");
                _errors.ForEach(e => errorMessage.AppendLine(e));
                throw new InvalidOperationException(errorMessage.ToString());
            }
        }

        private static void DetermineHost(IConfigurationSection configSection)
        {
            _host = configSection["Host"];
            if (string.IsNullOrEmpty(_host))
            {
                _errors.Add("Required config-setting 'Host' not found.");
                _isValid = false;
            }
        }

        private static void DeterminePort(IConfigurationSection configSection)
        {
            string portSetting = configSection["Port"];
            if (string.IsNullOrEmpty(portSetting))
            {
                _port = DEFAULT_PORT;
            }
            else
            {
                if (int.TryParse(portSetting, out int result))
                {
                    _port = result;
                }
                else
                {
                    _isValid = false;
                    _errors.Add("Unable to parse config-setting 'Port' into an integer.");
                }
            }
        }

        private static void DetermineExchange(IConfigurationSection configSection)
        {
            _exchange = configSection["Exchange"];
            if (string.IsNullOrEmpty(_exchange))
            {
                _isValid = false;
                _errors.Add("Required config-setting 'Exchange' not found.");
            }
        }

        private static void DetermineQueue(IConfigurationSection configSection)
        {
            _queue = configSection["Queue"];
            if (string.IsNullOrEmpty(_queue))
            {
                _isValid = false;
                _errors.Add("Required config-setting 'Queue' not found.");
            }
        }

        private static void DetermineVirtualHost(IConfigurationSection configSection)
        {
            string vhostSetting = configSection["VirtualHost"];
            if (string.IsNullOrEmpty(vhostSetting))
            {
                _virtualHost = DEFAULT_VIRTUAL_HOST;
            }
            else
            {
                _virtualHost = configSection["VirtualHost"];
            }
        }

        private static void DetermineRoutingKey(IConfigurationSection configSection)
        {
            _routingKey = configSection["RoutingKey"] ?? "";
        }
    }
}
