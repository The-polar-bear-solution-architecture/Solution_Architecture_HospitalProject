using AppointmentService.Domain;
using PatientService.Domain;
using PatientService.DomainServices;
using RabbitMQ.Infrastructure.MessageHandlers;
using RabbitMQ.Messages.Interfaces;
using RabbitMQ.Messages.Mapper;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace PatientService.Controllers
{
    public class PatientWorker : IMessageHandleCallback, IHostedService
    {
        private IReceiver _messageHandler;
        private IPublisher publisher;
        private IPatientRepository patientRepository;
        private IGeneralPractitionerRepository generalPractitionerRepository;

        public PatientWorker(IReceiver messageHandler, IPublisher publisher, IPatientRepository patientRepository, IGeneralPractitionerRepository generalPractitionerRepository)
        {
            _messageHandler = messageHandler;
            this.publisher = publisher;
            this.generalPractitionerRepository = generalPractitionerRepository;
            this.patientRepository = patientRepository;

        }


        public async Task<bool> HandleMessageAsync(string messageType, object message)
        {
            try
            {
                byte[] body = message as byte[];
                var externalEvent = JsonSerializer.Deserialize<ExternalPatientEvent>(body);
                var gp = generalPractitionerRepository.GetByEmail("tristan@mail.com");
                if (gp == null)
                {
                    return false;
                }
                foreach (ImportPatient patient in externalEvent.patientList)
                {
                    var existingPatient = patientRepository.GetByDetails(patient.FirstName, patient.LastName, patient.PhoneNumber);
                    if (existingPatient == null)
                    {
                        var x = new Patient();
                        x.Id = Guid.NewGuid();
                        x.GeneralPractitioner = gp;
                        x.FirstName = patient.FirstName;
                        x.LastName = patient.LastName;
                        x.Address = patient.Address;
                        x.PhoneNumber = patient.PhoneNumber;
                        patientRepository.Post(x);
                        await publisher.SendMessage("POST", x, "Created_Patient");
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task SendMessageAsync(string messageType, object message)
        {
            if (messageType == "POST")
            {
                await publisher.SendMessage(messageType, message, "Created_Patient");
            }
            if (messageType == "PUT")
            {
                await publisher.SendMessage(messageType, message, "Updated_Patient");
            }
            if (messageType == "DELETE")
            {
                await publisher.SendMessage(messageType, message, "Deleted_Patient");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Start(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Stop();
            return Task.CompletedTask;
        }
    }
}
