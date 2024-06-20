using Microsoft.EntityFrameworkCore;

namespace PatientService.Domain
{
    public class PatientDBContext : DbContext
    {
        public PatientDBContext(DbContextOptions<PatientDBContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<GeneralPractitioner> GeneralPractitioners { get; set; }
    }
}
