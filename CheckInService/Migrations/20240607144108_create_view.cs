using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckInService.Migrations
{
    /// <inheritdoc />
    public partial class create_view : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE OR ALTER VIEW AppointmentCheckInView AS\r\nSELECT checkIns.Id, Status, Appointments.AppointmentDate, Patients.FirstName AS PatientFirstName, Patients.LastName AS PatientLastName, Physicians.FirstName AS PhysicianFirstName, Physicians.LastName AS PhysicianLastName, Physicians.Email AS PhysiciansEmail\r\nFROM checkIns \r\nJOIN Appointments ON Appointments.Id = checkIns.AppointmentId\r\nJOIN Patients ON Appointments.PatientId = Patients.Id\r\nJOIN Physicians ON Appointments.PhysicianId = Physicians.Id;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW AppointmentCheckInView;");
        }
    }
}
