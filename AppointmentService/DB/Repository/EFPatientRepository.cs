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

        public Patient AddPatient(Patient Patient)
        {
            context.Add(Patient);
            context.SaveChanges();
            return Patient;
        }

        public Patient DeletePatient(Guid Id)
        {
            var patient = GetPatientById(Id);
            context.Remove(patient);
            context.SaveChanges();
            return patient;
        }

        public Patient GetPatientById(Guid Id)
        {
            return context.Patients.Where(p => p.Id == Id).FirstOrDefault();
        }

        public Patient UpdatePatient(Patient Patient)
        {
            context.Update(Patient);
            context.SaveChanges();
            return Patient;
        }
    }
}
