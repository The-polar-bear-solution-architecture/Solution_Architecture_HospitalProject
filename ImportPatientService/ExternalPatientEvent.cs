using RabbitMQ.Messages.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportPatientService
{
    public class ExternalPatientEvent: Event
    {
        public List<Patient> patientList {  get; init; }
    }
}
