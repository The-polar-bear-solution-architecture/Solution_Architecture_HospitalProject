using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckInService.Migrations.CheckInReadContextDBMigrations
{
    /// <inheritdoc />
    public partial class create_read_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckInReadModel",
                columns: table => new
                {
                    CheckInSerialNr = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckInId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AppointmentGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    ApointmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    PatientGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhysicianId = table.Column<int>(type: "int", nullable: false),
                    PhysicianGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhysicianFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhysicianLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhysicianEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckInReadModel", x => x.CheckInSerialNr);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckInReadModel");
        }
    }
}
