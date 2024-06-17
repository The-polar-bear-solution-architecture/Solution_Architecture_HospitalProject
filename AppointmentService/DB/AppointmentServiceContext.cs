using AppointmentService.Domain;
using Microsoft.EntityFrameworkCore;
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
        //public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Data Source=localhost;Initial Catalog=AppointmentService;User ID=sa;Password=Rick@Sanchez;Trust Server Certificate=True";
            string connectionStringJascha = "Data Source=.;Initial Catalog=AppointmentService;Integrated Security=True;Encrypt=False;Trust Server Certificate=True";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
