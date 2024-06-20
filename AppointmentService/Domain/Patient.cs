using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppointmentService.Domain
{
    public class Patient
    {

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        
        [JsonIgnore]
        public ICollection<Appointment> appointments { get; set; }
        public GeneralPractitioner GP { get; set; }



        public Patient(Guid id, string firstName, string lastName, string phoneNumber)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;

        }

        public Patient()
        {

        }

    }
}