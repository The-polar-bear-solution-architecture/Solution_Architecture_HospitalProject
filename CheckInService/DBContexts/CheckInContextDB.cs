using CheckinService.Model;
using CheckInService.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckInService.DBContexts
{
    public class CheckInContextDB : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Physician> Physicians { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<CheckIn> checkIns {  get; set; }

        public CheckInContextDB(DbContextOptions<CheckInContextDB> dbContext): base(dbContext)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=CheckInDB;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Appointment>().HasOne<Physician>(physician => physician.Physician);
            modelBuilder.Entity<Appointment>().HasOne<Patient>(patient => patient.Patient);
            modelBuilder.Entity<CheckIn>().HasOne<Appointment>(appointment => appointment.Appointment);
        }
    }
}
