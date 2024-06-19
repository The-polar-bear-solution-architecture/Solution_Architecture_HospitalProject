using AppointmentService.Domain;
using AppointmentService.DomainServices;

namespace AppointmentService.DB.Repository
{
    public class EFPhysicianRepository : IPhysicianRepository
    {
        private readonly AppointmentServiceContext context;

        public EFPhysicianRepository(AppointmentServiceContext context)
        {
            this.context = context; 
        }

        public Physician GetPhysicianById(Guid Id)
        {
            return context.Physicians.Where(p => p.Id == Id).FirstOrDefault();
        }
    }
}
