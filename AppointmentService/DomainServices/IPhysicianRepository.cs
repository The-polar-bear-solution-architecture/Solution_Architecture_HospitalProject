using AppointmentService.Domain;

namespace AppointmentService.DomainServices
{
    public interface IPhysicianRepository
    {
        public Physician GetPhysicianById(Guid Id);
    }
}
