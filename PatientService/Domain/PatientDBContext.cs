using Microsoft.EntityFrameworkCore;
using Polly;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Create a default general practicioner.
            GeneralPractitioner generalPractitioner = new GeneralPractitioner() { Email = "", FirstName = "Tristan", LastName = "Schuring",  Address = "Example street 123", PhoneNumber = "0312312413", Id = Guid.Parse("99463e24-c0cc-45d2-9b97-3e23e3556de3") };
            modelBuilder.Entity<GeneralPractitioner>().HasData(generalPractitioner);
        }

        public void MigrateDB()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetry(10, r => TimeSpan.FromSeconds(10))
                .Execute(() => Database.Migrate());
        }
    }
}
