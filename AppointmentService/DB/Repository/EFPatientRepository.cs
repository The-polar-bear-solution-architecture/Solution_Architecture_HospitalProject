using AppointmentService.Domain;
using AppointmentService.DomainServices;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Patient> DeletePatient(Guid Id)
        {
            var patient = GetPatientById(Id);
            Console.WriteLine(patient.Id);
            context.Remove(patient);
            await context.SaveChangesAsync();
            return patient;
        }

        public Patient GetPatientById(Guid Id)
        {
            var patient = context.Patients.Include(p => p.GP).Where(p => p.Id.Equals(Id)).FirstOrDefault();
            Console.WriteLine(patient.Id);
            return patient;
            
        }

        public Patient UpdatePatient(Patient Patient)
        {
            context.Update(Patient);
            context.SaveChanges();
            return Patient;
        }
    }
}
