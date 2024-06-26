﻿using Microsoft.AspNetCore.Identity;

namespace AppointmentService.Domain
{
    public class Physician
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public Physician(Guid id, string  firstName, string lastName, Role role, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;    
            Email = email;
            Role = role;
        } 

    }
}