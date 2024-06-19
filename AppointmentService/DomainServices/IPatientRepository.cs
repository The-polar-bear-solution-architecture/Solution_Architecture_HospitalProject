using AppointmentService.Domain;

namespace AppointmentService.DomainServices
{
    public interface IPatientRepository
    {
        public Patient GetPatientById(Guid Id); 
    }
}
