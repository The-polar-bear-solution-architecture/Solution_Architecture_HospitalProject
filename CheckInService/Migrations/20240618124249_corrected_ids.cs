using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckInService.Migrations
{
    /// <inheritdoc />
    public partial class corrected_ids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppointmentSerialNr",
                table: "Physicians",
                newName: "PhysicianSerialNr");

            migrationBuilder.RenameColumn(
                name: "AppointmentSerialNr",
                table: "Patients",
                newName: "PatientSerialNr");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhysicianSerialNr",
                table: "Physicians",
                newName: "AppointmentSerialNr");

            migrationBuilder.RenameColumn(
                name: "PatientSerialNr",
                table: "Patients",
                newName: "AppointmentSerialNr");
        }
    }
}
