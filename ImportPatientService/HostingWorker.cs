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
            this.publisher = new RabbitMQPublisher("rabbit", "Hospital_Brenda", 5672, "/");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            List<Patient> list = new List<Patient>();
            int i = 0;
            while (i < 2)
            {
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
                publisher.SendMessage("CSVPatient", externalEvent, "Import_Customers");
                
                i++;
                Console.WriteLine($"Run geeindigt is op index {i}");
            }
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
