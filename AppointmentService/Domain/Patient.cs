using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppointmentService.Domain
{
    public class Patient
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string BSN { get; set; }
        [JsonIgnore]
        public ICollection<Appointment> appointments { get; set; }
        public GeneralPractitioner GP { get; set; }



        public Patient(int id, string firstName, string lastName, DateTime dateOfBirth, string email, string bSN)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Email = email;
            BSN = bSN;
        }
    }
}