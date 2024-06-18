using CsvHelper;
using ImportPatientService;
using System.Formats.Asn1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using RabbitMQ.Messages.Configuration;
using RabbitMQ.Messages.Interfaces;
using RabbitMQ.Infrastructure.MessagePublishers;


IHost host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // services.UseRabbitMQMessagePublisher(hostContext.Configuration);
        services.AddHostedService<HostingWorker>();
    })
    .UseConsoleLifetime()
    .Build();
await host.RunAsync();
