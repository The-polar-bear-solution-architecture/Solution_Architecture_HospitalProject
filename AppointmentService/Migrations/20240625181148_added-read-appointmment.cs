using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentService.Migrations
{
    /// <inheritdoc />
    public partial class addedreadappointmment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "appointmentsRead",
                columns: table => new
                {
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhysicianId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GPId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreviousAppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhysicianFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhysicianLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhysicianEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhysicianRole = table.Column<int>(type: "int", nullable: false),
                    GPFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GPFlastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GPEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointmentsRead", x => x.AppointmentId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointmentsRead");
        }
    }
}
