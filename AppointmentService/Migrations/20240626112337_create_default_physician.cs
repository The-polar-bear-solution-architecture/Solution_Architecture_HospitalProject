using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentService.Migrations
{
    /// <inheritdoc />
    public partial class create_default_physician : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Physicians",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Role" },
                values: new object[] { new Guid("a1d4539f-82c6-4531-9296-7c34b5cdf1d5"), "Franks@example.com", "Diederik", "Franks", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Physicians",
                keyColumn: "Id",
                keyValue: new Guid("a1d4539f-82c6-4531-9296-7c34b5cdf1d5"));
        }
    }
}
