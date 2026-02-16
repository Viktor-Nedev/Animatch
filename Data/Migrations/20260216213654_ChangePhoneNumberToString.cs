using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Animatch.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangePhoneNumberToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://miau.bg/files/1200x800/nemska-ovcharka2.webp");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://animoetc.com/cdn/shop/articles/AdobeStock_67967166.jpg?v=1743684891");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSWrp8aabL_Woah30nQChpwR1HbfGZdROmv3ceN8c8sjVTZYdVMnBLQeDZ8dFmG43mC00MIveiakCHE2LSy4bk88kU03YhB9QlgtDLXBw&s=10");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/a/a3/Black-Magic-Big-Boy.jpg");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "https://m.netinfo.bg/media/images/50202/50202954/624-400-ulichna-kotka.jpg");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "https://cdn.marica.bg/images/marica.bg/889/1200_cherna-kotka-spasitelka.jpg");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS3uIKtBwcbAwSi6o_uzUqgp016-A3D4bswPQ&s");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "https://static.bnr.bg/gallery/b9/b9ab15c0df0ee58648c4c8d48aa7f9c3.jpg");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 12,
                column: "ImageUrl",
                value: "https://miau.bg/files/1200x800/cacadu.webp");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 13,
                column: "ImageUrl",
                value: "https://miau.bg/files/lib/500x350/baby-rabbit-carrot1.webp");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 21,
                column: "ImageUrl",
                value: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQtyukiUUwuqFgAHnUDE9QkkiUetBo50Uwnzw&s");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 29,
                column: "ImageUrl",
                value: "https://www.kakvoe.eu/wp-content/uploads/2020/06/%D1%82%D0%B0%D1%80%D0%B0%D0%BB%D0%B5%D0%B6.jpeg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://placehold.co/600x400?text=Dog");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://placehold.co/600x400?text=Dog");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://placehold.co/600x400?text=Dog");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://placehold.co/600x400?text=Dog");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "https://placehold.co/600x400?text=Cat");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "https://placehold.co/600x400?text=Cat");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "https://placehold.co/600x400?text=Cat");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "https://placehold.co/600x400?text=Parrot");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 12,
                column: "ImageUrl",
                value: "https://placehold.co/600x400?text=Cockatoo");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 13,
                column: "ImageUrl",
                value: "https://placehold.co/600x400?text=Rabbit");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 21,
                column: "ImageUrl",
                value: "https://placehold.co/600x400?text=Lizard");

            migrationBuilder.UpdateData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 29,
                column: "ImageUrl",
                value: "https://placehold.co/600x400?text=Hedgehog");
        }
    }
}
