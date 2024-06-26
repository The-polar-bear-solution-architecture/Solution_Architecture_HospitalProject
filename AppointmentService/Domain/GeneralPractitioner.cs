﻿namespace AppointmentService.Domain
{
    public class GeneralPractitioner
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email {  get; set; }
        public GeneralPractitioner(Guid id, string firstName, string lastName, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;  
        }
    }
}