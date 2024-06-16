using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckInService.Migrations
{
    /// <inheritdoc />
    public partial class create_extra_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerialNr",
                table: "checkIns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialNr",
                table: "checkIns");
        }
    }
}
