using AppointmentService.Domain;
using AppointmentService.DomainServices;
using Microsoft.EntityFrameworkCore;

namespace AppointmentService.DB.Repository
{
    public class EFAppointmentRepository : IAppointmentRepository
    {

        private readonly AppointmentServiceContext context;

        public EFAppointmentRepository(AppointmentServiceContext context)
        {
            this.context = context;
        }

        public Appointment AddAppointment(Appointment appointment)
        {
            context.Appointments.Add(appointment);
            context.SaveChanges();
            return appointment;
        }

        public void DeleteAppointment(int id)
        {
            context.Remove(GetAppointmentById(id));
            context.SaveChanges();
        }

        public IEnumerable<Appointment> GetAllAppointments()
        {
            return context.Appointments.Include(c => c.Patient).Include(c => c.Physician).Include(c => c.PreviousAppointment).Include(c => c.Patient.GP);
        }

        public Appointment GetAppointmentById(int id)
        {
            return context.Appointments.Include(c => c.Patient).Include(c => c.Physician).Include(c => c.PreviousAppointment).Include(c => c.Patient.GP).Where(a => a.Id == id).FirstOrDefault(); 
        }

        public void UpdateAppointment(Appointment appointment)
        {
            //var edit = GetAppointmentById(appointment.Id);
            context.Update(appointment);
            //context.Update(edit);
            //edit = appointment;
            context.SaveChanges();
        }
    }
}
