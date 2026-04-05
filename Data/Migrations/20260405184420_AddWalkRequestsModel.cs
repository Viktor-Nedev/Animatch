using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Animatch.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWalkRequestsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WalkRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    RequesterId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequestedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalkRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalkRequests_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "WalkRequests",
                columns: new[] { "Id", "AnimalId", "Message", "RequestedOn", "RequesterId", "Status" },
                values: new object[,]
                {
                    { 1, 1, "Свободен съм в събота следобед за разходка.", new DateTime(2026, 4, 1, 14, 0, 0, 0, DateTimeKind.Utc), "seed-user-1", 0 },
                    { 2, 2, "Мога да помогна в неделя сутрин.", new DateTime(2026, 4, 2, 9, 30, 0, 0, DateTimeKind.Utc), "seed-user-2", 1 },
                    { 3, 6, "Искам да участвам и в следващи инициативи.", new DateTime(2026, 4, 3, 11, 15, 0, 0, DateTimeKind.Utc), "seed-user-3", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WalkRequests_AnimalId",
                table: "WalkRequests",
                column: "AnimalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalkRequests");
        }
    }
}
