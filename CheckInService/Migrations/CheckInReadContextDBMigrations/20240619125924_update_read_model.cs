using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckInService.Migrations.CheckInReadContextDBMigrations
{
    /// <inheritdoc />
    public partial class update_read_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "CheckInReadModel");

            migrationBuilder.DropColumn(
                name: "CheckInId",
                table: "CheckInReadModel");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "CheckInReadModel");

            migrationBuilder.DropColumn(
                name: "PhysicianId",
                table: "CheckInReadModel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "CheckInReadModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CheckInId",
                table: "CheckInReadModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "CheckInReadModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PhysicianId",
                table: "CheckInReadModel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
