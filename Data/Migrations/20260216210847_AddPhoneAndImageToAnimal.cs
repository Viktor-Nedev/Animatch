using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Animatch.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoneAndImageToAnimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Animals",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Animals",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ImageUrl", "PhoneNumber" },
                values: new object[] { "https://placehold.co/600x400?text=Dog", "0881111111" });

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ImageUrl", "PhoneNumber" },
                values: new object[] { "https://placehold.co/600x400?text=Dog", "0882222222" });

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ImageUrl", "PhoneNumber" },
                values: new object[] { "https://placehold.co/600x400?text=Dog", "0883333333" });

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ImageUrl", "PhoneNumber" },
                values: new object[] { "https://placehold.co/600x400?text=Dog", "0884444444" });

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ImageUrl", "PhoneNumber" },
                values: new object[] { "https://placehold.co/600x400?text=Cat", "0885555555" });

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ImageUrl", "PhoneNumber" },
                values: new object[] { "https://placehold.co/600x400?text=Cat", "0886666666" });

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ImageUrl", "PhoneNumber" },
                values: new object[] { "https://placehold.co/600x400?text=Cat", "0887777777" });

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ImageUrl", "PhoneNumber" },
                values: new object[] { "https://placehold.co/600x400?text=Parrot", "0888888888" });

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ImageUrl", "PhoneNumber" },
                values: new object[] { "https://placehold.co/600x400?text=Cockatoo", "0889999999" });

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "ImageUrl", "PhoneNumber" },
                values: new object[] { "https://placehold.co/600x400?text=Rabbit", "0870123456" });

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "ImageUrl", "PhoneNumber" },
                values: new object[] { "https://placehold.co/600x400?text=Lizard", "0871234567" });

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "ImageUrl", "PhoneNumber" },
                values: new object[] { "https://placehold.co/600x400?text=Hedgehog", "0872345678" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Animals");
        }
    }
}
