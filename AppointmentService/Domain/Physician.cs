using Microsoft.AspNetCore.Identity;

namespace AppointmentService.Domain
{
    public class Physician
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Role Role { get; set; }
        public Physician(int id, string  firstName, string lastName, Role role)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;    
            Role = role;
        } 

    }
}