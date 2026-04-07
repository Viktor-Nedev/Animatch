using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Animatch.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrganizationProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Town = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationProfiles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "OrganizationProfiles",
                columns: new[] { "Id", "CreatedOn", "Description", "LogoUrl", "OrganizationName", "PhoneNumber", "Town", "UserId", "WebsiteUrl" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 10, 9, 0, 0, 0, DateTimeKind.Utc), "Доброволческа организация за разходки на кучета, временни домове и транспорт до клиники.", "https://images.unsplash.com/photo-1601758228041-f3b2795255f1?auto=format&fit=crop&w=800&q=80", "Happy Paws Foundation", "0888123456", "Sofia", "seed-organizer-1", "https://happypaws.example.org" },
                    { 2, new DateTime(2026, 1, 15, 11, 30, 0, 0, DateTimeKind.Utc), "Екип, който координира кампании за хранене, кастрация и социализация на бездомни животни.", "https://images.unsplash.com/photo-1554692936-19b9b7f0f89a?auto=format&fit=crop&w=800&q=80", "Stray Care Collective", "0899765432", "Plovdiv", "seed-organizer-2", "https://straycare.example.org" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationProfiles_UserId",
                table: "OrganizationProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationProfiles");
        }
    }
}
