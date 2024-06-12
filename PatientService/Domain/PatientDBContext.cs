using Microsoft.EntityFrameworkCore;

namespace PatientService.Domain
{
    public class PatientDBContext : DbContext
    {
        public PatientDBContext(DbContextOptions<PatientDBContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=Braphia_PatientService;Trusted_Connection=True;ConnectRetryCount=0");
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<GeneralPractitioner> GeneralPractitioners { get; set; }
    }
}
