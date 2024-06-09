using AppointmentService.Domain;
using AppointmentService.DomainServices;

namespace AppointmentService.DB.Repository
{
    public class EFPatientRepository : IPatientRepository
    {
        private readonly AppointmentServiceContext context;

        public EFPatientRepository(AppointmentServiceContext context)
        {
            this.context = context;
        }
        public Patient GetPatientById(int Id)
        {
            return context.Patients.Where(p => p.Id == Id).FirstOrDefault();
        }
    }
}
