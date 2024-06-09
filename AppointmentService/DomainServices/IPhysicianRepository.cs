using AppointmentService.Domain;

namespace AppointmentService.DomainServices
{
    public interface IPhysicianRepository
    {
        public Physician GetPhysicianById(int Id);
    }
}
