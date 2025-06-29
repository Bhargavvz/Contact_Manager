using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContactManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "Address", "DateCreated", "Email", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, "123 Main St, New York, NY 10001", new DateTime(2025, 6, 30, 0, 25, 10, 960, DateTimeKind.Local).AddTicks(8423), "john.doe@example.com", "John Doe", "+1-555-0123" },
                    { 2, "456 Oak Ave, Los Angeles, CA 90210", new DateTime(2025, 6, 30, 0, 25, 10, 960, DateTimeKind.Local).AddTicks(8861), "jane.smith@example.com", "Jane Smith", "+1-555-0456" },
                    { 3, "789 Pine St, Chicago, IL 60601", new DateTime(2025, 6, 30, 0, 25, 10, 960, DateTimeKind.Local).AddTicks(8896), "bob.johnson@example.com", "Bob Johnson", "+1-555-0789" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
