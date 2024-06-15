using AppointmentService.Domain;

namespace AppointmentService.DomainServices
{
    public interface IAppointmentRepository
    {
        public Appointment AddAppointment(Appointment appointment);
        public Appointment GetAppointmentById(int id);
        public IEnumerable<Appointment> GetAllAppointments();
        public void UpdateAppointment(Appointment appointment);
        public void DeleteAppointment(int id);
    }
}
