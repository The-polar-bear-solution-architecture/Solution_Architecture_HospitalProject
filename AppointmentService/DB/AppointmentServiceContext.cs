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


        public void MigrateDB()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetry(10, r => TimeSpan.FromSeconds(10))
                .Execute(() => Database.Migrate());
        }
    }
}
