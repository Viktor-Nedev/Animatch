using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Animatch.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerIdToAnimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Animals",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Animals");
        }
    }
}
