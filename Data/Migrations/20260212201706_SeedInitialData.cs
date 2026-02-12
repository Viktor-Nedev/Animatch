using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Animatch.Migrations.AnimalManagerDb
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Кучета" },
                    { 2, "Котки" },
                    { 3, "Птици" },
                    { 4, "Зайци" },
                    { 5, "Гризачи" },
                    { 6, "Риби" },
                    { 7, "Влечуги" },
                    { 8, "Земноводни" },
                    { 9, "Конe" },
                    { 10, "Селскостопански" },
                    { 11, "Диви животни" }
                });

            migrationBuilder.InsertData(
                table: "Animals",
                columns: new[] { "Id", "Breed", "CategoryId", "Description", "Name", "Species", "Town" },
                values: new object[,]
                {
                    { 1, "Немска овчарка", 1, "Търся приятел за дълги разходки в Борисовата градина. Обичам да гоня топка и да тичам до езерото. Имам безкрайно много енергия!", "Рекс", "Куче", "София" },
                    { 2, "Златен ретривър", 1, "Супер дружелюбна, обичам всички. Търся куче за игра в парк 'Лаута'. Нося си топка и фризби. Обичам и плуване!", "Бела", "Куче", "Пловдив" },
                    { 3, "Джак Ръсел", 1, "Малък, но бърз! Търся приятел за гоненица по плажа. Обичам да ровя в пясъка и да лая на вълните.", "Боби", "Куче", "Варна" },
                    { 4, "Хъски", 1, "Обожавам снега! Търся друго хъски или активно куче за планински преходи. Карам и шейна (шегувам се... или не?).", "Луна", "Куче", "Банско" },
                    { 6, "Улична принцеса", 2, "Търся коте за приятел. Обичам да гледам птици заедно и да спим на слънце. Нося ти подаръци (мишки).", "Гизда", "Котка", "София" },
                    { 7, "Черна", 2, "Търся спокойно коте за приятел. Обичам да се търкалям по топлите плочки и да ям риба. Не съм за врачка!", "Салем", "Котка", "Варна" },
                    { 9, "Сиамска", 2, "Елегантна и разговорлива. Търся приятел, който обича да 'разговаря'. Мяукам на различни мелодии!", "Зора", "Котка", "Бургас" },
                    { 10, "Африкански сив", 3, "Говоря 50 думи. Търся приятел за разговори и свирене. Ще те науча да казваш 'Здрасти'!", "Жако", "Папагал", "София" },
                    { 12, "Бял", 3, "Танцувам! Сериозно, имам ритъм. Търся приятел, който да ме гледа как се клатя. Обичам фъстъци.", "Пио", "Какаду", "Пловдив" },
                    { 13, "Джудже", 4, "Търся зайче за приятел. Обичам да скачам, да гриза моркови и да си ровя в сеното. Ще тичаме заедно!", "Хоп", "Заек", "Бургас" },
                    { 21, "Брадат дракон", 7, "Търся друг гущер за приятел. Обичам да се пека на лампата и да ям скакалци. Махам си с ръка за поздрав.", "Спайк", "Гущер", "Хасково" },
                    { 29, "Див", 11, "Дойдох сам в градината. Търся приятел за вечерни разходки. Ям кучешка храна и съм много сладък.", "Бодльо", "Таралеж", "Ловеч" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11);
        }
    }
}
