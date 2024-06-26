using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentService.Migrations
{
    /// <inheritdoc />
    public partial class removedreadmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointmentsRead");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "appointmentsRead",
                columns: table => new
                {
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GPEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GPFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GPFlastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GPId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhysicianEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhysicianFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhysicianId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhysicianLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhysicianRole = table.Column<int>(type: "int", nullable: false),
                    PreviousAppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointmentsRead", x => x.AppointmentId);
                });
        }
    }
}
