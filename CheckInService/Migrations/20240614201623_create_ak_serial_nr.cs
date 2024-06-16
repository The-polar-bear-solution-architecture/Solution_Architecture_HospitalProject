using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckInService.Migrations
{
    /// <inheritdoc />
    public partial class create_ak_serial_nr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SerialNr",
                table: "checkIns",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_checkIns_SerialNr",
                table: "checkIns",
                column: "SerialNr");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_checkIns_SerialNr",
                table: "checkIns");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNr",
                table: "checkIns",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
