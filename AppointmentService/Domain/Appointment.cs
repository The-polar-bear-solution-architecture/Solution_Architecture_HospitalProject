namespace AppointmentService.Domain
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Physician Physician { get; set; }
        public Patient Patient { get; set; }
        public Appointment? PreviousAppointment { get; set; }

        //public Appointment (int id, string name, DateTime appointmentDate, Physician physician, Patient patient, Appointment? previousAppointment)
        //{
        //    Id = id;
        //    Name = name;
        //    AppointmentDate = appointmentDate;
        //    Physician = physician;
        //    Patient = patient;
        //    PreviousAppointment = previousAppointment;
        //}
    }
}
