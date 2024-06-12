// See https://aka.ms/new-console-template for more information
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using System.Net;
global using System.Net.Mail;
global using System.Net.Mime;
global using System.Text;
global using Polly;
global using System.Data.SqlClient;
using RabbitMQ.Messages.Configuration;
using TestHandler;

Console.WriteLine("Hello, World!");
IHost host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.UseRabbitMQMessageHandler(hostContext.Configuration);
        services.AddHostedService<HostingWorker>();
    })
    .UseConsoleLifetime()
    .Build();

await host.RunAsync();