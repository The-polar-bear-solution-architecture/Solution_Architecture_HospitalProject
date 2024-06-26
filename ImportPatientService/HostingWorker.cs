using CsvHelper;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Infrastructure.MessagePublishers;
using RabbitMQ.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ImportPatientService
{
    public class HostingWorker : IHostedService
    {
        private IPublisher publisher;
        private readonly HttpClient httpClient;

        public HostingWorker()
        {
            this.httpClient = new HttpClient();
            this.publisher = new RabbitMQPublisher("rabbit", "Hospital_Brenda_Patient", 5672, "/");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            List<Patient> list = new List<Patient>();

            var raw = HTTPClient.GetPatientsFromCSV(httpClient).Result;
            using (var reader = new StreamReader(raw))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var patients = csv.GetRecords<Patient>();
                foreach (var value in patients)
                {
                    Console.WriteLine(value.FirstName);
                    list.Add(value);
                }
            }

            ExternalPatientEvent externalEvent = new ExternalPatientEvent() { patientList = list };
            await publisher.SendMessage("CSVPatient", externalEvent, "Import_Customers");

            Console.WriteLine("Run ended.");

            // Optionally, you can clear the list if it's needed for subsequent operations.
            list.Clear();

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
