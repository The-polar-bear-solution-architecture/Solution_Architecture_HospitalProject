using CheckinService.Model;
using CheckInService.Models;
using CheckInService.Queries;
using Microsoft.EntityFrameworkCore;

namespace CheckInService.DBContexts
{
    public class CheckInContextDB : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Physician> Physicians { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<CheckIn> checkIns {  get; set; }

        // View
        public DbSet<CheckInView> checkInsView { get; set; }

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

            
            modelBuilder.Entity<Appointment>().HasOne(physician => physician.Physician);
            modelBuilder.Entity<Appointment>().HasOne(patient => patient.Patient);
            
            modelBuilder.Entity<CheckIn>().HasOne<Appointment>(appointment => appointment.Appointment);
            modelBuilder.Entity<CheckIn>().HasAlternateKey(ci => ci.SerialNr);

            CreateViews(modelBuilder);
        }

        private void CreateViews(ModelBuilder modelBuilder)
        {
            // If name change is required, go to migrations folder, or copy this query to the new Migrations folder.
            /* CREATE OR ALTER VIEW AppointmentCheckInView AS
            SELECT checkIns.Id, Status, Patients.FirstName AS PatientFirstName, Patients.LastName AS PatientLastName, Physicians.FirstName AS PhysicianFirstName, Physicians.LastName AS PhysicianLastName, Physicians.Email AS PhysiciansEmail
            FROM checkIns
            JOIN Appointments ON Appointments.Id = checkIns.AppointmentId
            JOIN Patients ON Appointments.PatientId = Patients.Id
            JOIN Physicians ON Appointments.PhysicianId = Physicians.Id; */
            modelBuilder.Entity<CheckInView>().HasNoKey().ToView("AppointmentCheckInView");
        }
    }
}
