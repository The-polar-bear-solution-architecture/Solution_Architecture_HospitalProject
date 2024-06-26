using AppointmentService.Domain;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Collections.Generic;

namespace AppointmentService.DB
{
    public class AppointmentServiceContext : DbContext
    {

        public AppointmentServiceContext(DbContextOptions<AppointmentServiceContext> options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<GeneralPractitioner> GeneralPractitioners { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Physician> Physicians { get; set; }
        public DbSet<AppointmentRead> appointmentsRead { get; set; }

        //public DbSet<Role> Roles { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=AppointmentService;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Physician defaultPhysician = new Physician(Guid.Parse("a1d4539f-82c6-4531-9296-7c34b5cdf1d5"), "Diederik", "Franks", Role.Cardiology, "Franks@example.com");

            modelBuilder.Entity<Physician>().HasData(defaultPhysician);
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
