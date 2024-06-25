using AppointmentService.Domain;

namespace PatientService.Domain
{
    public class ExternalPatientEvent
    {
        public List<ImportPatient> patientList { get; init; }
    }
}
