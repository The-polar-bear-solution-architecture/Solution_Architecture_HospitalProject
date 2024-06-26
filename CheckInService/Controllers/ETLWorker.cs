using CheckInService.CommandHandlers;
using CheckInService.CommandsAndEvents.Commands.Appointment;
using CheckInService.Models.DTO;
using CheckInService.Models;
using CheckInService.Repositories;
using RabbitMQ.Messages.Interfaces;
using RabbitMQ.Messages.Mapper;
using CheckInService.Mapper;
using RabbitMQ.Infrastructure.MessageHandlers;
using CheckInService.CommandsAndEvents.Commands.CheckIn;
using CheckInService.Pipelines;
using RabbitMQ.Messages.Messages;
using CheckInService.CommandsAndEvents.Events.Appointment;
using CheckInService.CommandsAndEvents.Events.CheckIn;
using CheckInService.Configurations;
using CheckInService.DBContexts;
using CheckInService.CommandsAndEvents.Events.Patient;
using CheckInService.CommandsAndEvents.Commands.Patient;

namespace CheckInService.Controllers
{
    public class ETLWorker : IMessageHandleCallback, IHostedService
    {
        private IReceiver _messageHandler;
        private readonly ReadModelRepository readModelRepository;
        private readonly CheckInPipeline checkInPipeline;
        private readonly IRabbitFactory RabbitFactory;
        private readonly PatientRepo patientRepo;
        private readonly CheckInCommandHandler checkInCommandHandler;
        private readonly PatientCommandHandler patientCommandHandler;

        public ETLWorker(
            ReadModelRepository readModelRepository,
            CheckInPipeline checkInPipeline,
            CheckInCommandHandler checkInCommandHandler,
            PatientCommandHandler patientCommandHandler,
            IRabbitFactory rabbitFactory,
            PatientRepo patientRepo)
        {
            
            this.readModelRepository = readModelRepository;
            this.checkInPipeline = checkInPipeline;
            this.RabbitFactory = rabbitFactory;
            this.patientRepo = patientRepo;
            this.checkInCommandHandler = checkInCommandHandler;
            this.patientCommandHandler = patientCommandHandler;
            // Will only receive messages via an internal exchange.
            _messageHandler = rabbitFactory.CreateInternalReceiver();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Create ETL Worker");
            _messageHandler.Start(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Stop();
            return Task.CompletedTask;
        }

        public async Task<bool> HandleMessageAsync(string messageType, object message)
        {
            byte[] data = message as byte[];
            if (messageType.Equals("Test"))
            {
                Console.WriteLine(data.Deserialize<string>());
                return true;
            }
            else
            {
                try
                {
                    TryRunIndividualPipeline(messageType, data);
                    TryCollectivePipeline(messageType, data);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public async void TryRunIndividualPipeline(string messageType, byte[] message)
        {
            byte[] data = message;
            if (messageType.Equals(nameof(CheckInRegistrationEvent)))
            {
                var deserializedData = data.Deserialize<CheckInReadModel>();
                // Perform operations.
                readModelRepository.Create(deserializedData);
            }
            else if (messageType.Equals(nameof(CheckInNoShowEvent)) || messageType.Equals(nameof(CheckInPresentEvent)))
            {
                var updateCheckIn = data.Deserialize<CheckInUpdateCommand>();
                // Perform operations.
                readModelRepository.Update(updateCheckIn);
            }
            else if (messageType.Equals(nameof(AppointmentUpdateEvent)))
            {
                var appointmentUpdate = data.Deserialize<AppointmentUpdateCommand>();
                // Perform operations.
                readModelRepository.Update(appointmentUpdate);
            }
            else if (messageType.Equals(nameof(AppointmentDeleteEvent)))
            {
                var appointmentDeletion = data.Deserialize<AppointmentDeleteCommand>();
                readModelRepository.DeleteByAppointment(appointmentDeletion.AppointmentId);
            }
            else if (messageType.Equals(nameof(PatientChangeEvent)))
            {
                var patient_update = data.Deserialize<PatientUpdate>();
                var checkins = readModelRepository.GetByPatient(patient_update.Id);
                checkins.ForEach(ch => { 
                    ch.PatientFirstName = patient_update.FirstName; 
                    ch.PatientLastName = patient_update.LastName; 
                    });

                // Update all patient models with new patient data.
                readModelRepository.BulkUpdate(checkins);
            }
            else
            {
                Console.WriteLine("No match found.");
            }
        }
        
        public async void TryCollectivePipeline(string messageType, byte[] message)
        {
            if (messageType.Equals("Clear"))
            {
                Console.WriteLine("Clear databases.");
                readModelRepository.DeleteAll();
                checkInCommandHandler.ClearAll();
            }
            else if (messageType.Equals("Replay"))
            {
                // First clear all data from read database
                readModelRepository.DeleteAll();
                checkInCommandHandler.ClearAll();

                // Fill Event source -> write database
                await checkInPipeline.ReplayDataPipeline();
                // Does currently nothing, but will synchronise with write database.
                await checkInPipeline.SynchroniseWriteDBWithReadDB();
            }
            else
            {
                Console.WriteLine("No match found. In collective pipeline.");
            }
        }
    }

    
}
