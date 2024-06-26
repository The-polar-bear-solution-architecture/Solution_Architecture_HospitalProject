using AppointmentService.Domain;

namespace AppointmentService.DomainServices
{
    public interface IAppointmentRepository
    {
        public AppointmentRead AddAppointment(Appointment appointment);
        public AppointmentRead GetAppointmentById(Guid id);
        public IEnumerable<AppointmentRead> GetAllAppointments();
        public AppointmentRead UpdateAppointment(Appointment appointment);
        public AppointmentRead DeleteAppointment(Guid id);
        public Appointment GetWriteAppointmentById(Guid id);
    }
}
