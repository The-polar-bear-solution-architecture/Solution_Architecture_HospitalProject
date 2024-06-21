﻿using AppointmentService.CommandsAndEvents.Commands;
using AppointmentService.CommandsAndEvents.Events;
using RabbitMQ.Messages.Interfaces;
using RabbitMQ.Messages.Mapper;
using RabbitMQ.Messages.Messages;

namespace AppointmentService.Controllers
{
    public class AppointmentWorker : IMessageHandleCallback, IHostedService
    {

        private IReceiver _messageReceiver;
        private readonly PatientCommandHandler _commandHandler;

        public AppointmentWorker(IReceiver messageReceiver, PatientCommandHandler commandHandler)
        {
            _messageReceiver = messageReceiver;
            _commandHandler = commandHandler;
        }

        public async Task<bool> HandleMessageAsync(string messageType, object message)
        {
             
            byte[] body = message as byte[];

            switch (messageType)
            {
                case "PatientCreated":
                    var patientCreated = body.Deserialize<PatientCreated>();
                    _commandHandler.PatientCreated(patientCreated);
                    break;
                case "PatientUpdated": 
                    var patientUpdated = body.Deserialize<PatientUpdated>();    
                    _commandHandler.PatientUpdated(patientUpdated);
                    break;
                case "PatientDeleted": 
                    var patientDeleted = body.Deserialize<PatientDeleted>();
                    _commandHandler.PatientDeleted(patientDeleted);
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    Console.WriteLine($"{messageType} {message}");
                    break;
            }

            return true;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting appointment worker");
            _messageReceiver.Start(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping appointment worker");
            _messageReceiver.Stop();
            return Task.CompletedTask;  

        }
    }
}
