using Animatch.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Animatch.Data.Configuration
{
	public class AnimalEntityConfiguration : IEntityTypeConfiguration<Animal>
	{
		private IEnumerable<Animal> animals = new List<Animal>()
		{
            new Animal
			{
				Id = 1,
				Name = "Рекс",
				Species = "Куче",
				Breed = "Немска овчарка",
				Town = "София",
				Description = "Търся приятел за дълги разходки в Борисовата градина. Обичам да гоня топка и да тичам до езерото. Имам безкрайно много енергия!",
				PhoneNumber = "0881111111",
				ImageUrl = "https://miau.bg/files/1200x800/nemska-ovcharka2.webp",
				CategoryId = 1
			},
			new Animal
			{
				Id = 2,
				Name = "Бела",
				Species = "Куче",
				Breed = "Златен ретривър",
				Town = "Пловдив",
				Description = "Супер дружелюбна, обичам всички. Търся куче за игра в парк 'Лаута'. Нося си топка и фризби. Обичам и плуване!",
				PhoneNumber = "0882222222",
				ImageUrl = "https://animoetc.com/cdn/shop/articles/AdobeStock_67967166.jpg?v=1743684891",
				CategoryId = 1
			},
			new Animal
			{
				Id = 3,
				Name = "Боби",
				Species = "Куче",
				Breed = "Джак Ръсел",
				Town = "Варна",
				Description = "Малък, но бърз! Търся приятел за гоненица по плажа. Обичам да ровя в пясъка и да лая на вълните.",
				PhoneNumber = "0883333333",
				ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSWrp8aabL_Woah30nQChpwR1HbfGZdROmv3ceN8c8sjVTZYdVMnBLQeDZ8dFmG43mC00MIveiakCHE2LSy4bk88kU03YhB9QlgtDLXBw&s=10",
				CategoryId = 1
			},
			new Animal
			{
				Id = 4,
				Name = "Луна",
				Species = "Куче",
				Breed = "Хъски",
				Town = "Банско",
				Description = "Обожавам снега! Търся друго хъски или активно куче за планински преходи. Карам и шейна (шегувам се... или не?).",
				PhoneNumber = "0884444444",
				ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/a/a3/Black-Magic-Big-Boy.jpg",
				CategoryId = 1
			},


            new Animal
			{
				Id = 6,
				Name = "Гизда",
				Species = "Котка",
				Breed = "Улична принцеса",
				Town = "София",
				Description = "Търся коте за приятел. Обичам да гледам птици заедно и да спим на слънце. Нося ти подаръци (мишки).",
				PhoneNumber = "0885555555",
				ImageUrl = "https://m.netinfo.bg/media/images/50202/50202954/624-400-ulichna-kotka.jpg",
				CategoryId = 2
			},
			new Animal
			{
				Id = 7,
				Name = "Салем",
				Species = "Котка",
				Breed = "Черна",
				Town = "Варна",
				Description = "Търся спокойно коте за приятел. Обичам да се търкалям по топлите плочки и да ям риба. Не съм за врачка!",
				PhoneNumber = "0886666666",
				ImageUrl = "https://cdn.marica.bg/images/marica.bg/889/1200_cherna-kotka-spasitelka.jpg",
				CategoryId = 2
			},

			new Animal
			{
				Id = 9,
				Name = "Зора",
				Species = "Котка",
				Breed = "Сиамска",
				Town = "Бургас",
				Description = "Елегантна и разговорлива. Търся приятел, който обича да 'разговаря'. Мяукам на различни мелодии!",
				PhoneNumber = "0887777777",
				ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS3uIKtBwcbAwSi6o_uzUqgp016-A3D4bswPQ&s",
				CategoryId = 2
			},

            new Animal
			{
				Id = 10,
				Name = "Жако",
				Species = "Папагал",
				Breed = "Африкански сив",
				Town = "София",
				Description = "Говоря 50 думи. Търся приятел за разговори и свирене. Ще те науча да казваш 'Здрасти'!",
				PhoneNumber = "0888888888",
				ImageUrl = "https://static.bnr.bg/gallery/b9/b9ab15c0df0ee58648c4c8d48aa7f9c3.jpg",
				CategoryId = 3
			},

			new Animal
			{
				Id = 12,
				Name = "Пио",
				Species = "Какаду",
				Breed = "Бял",
				Town = "Пловдив",
				Description = "Танцувам! Сериозно, имам ритъм. Търся приятел, който да ме гледа как се клатя. Обичам фъстъци.",
				PhoneNumber = "0889999999",
				ImageUrl = "https://miau.bg/files/1200x800/cacadu.webp",
				CategoryId = 3
			},   
            new Animal
			{
				Id = 13,
				Name = "Хоп",
				Species = "Заек",
				Breed = "Джудже",
				Town = "Бургас",
				Description = "Търся зайче за приятел. Обичам да скачам, да гриза моркови и да си ровя в сеното. Ще тичаме заедно!",
				PhoneNumber = "0870123456",
				ImageUrl = "https://miau.bg/files/lib/500x350/baby-rabbit-carrot1.webp",
				CategoryId = 4
			},

			new Animal
			{
				Id = 21,
				Name = "Спайк",
				Species = "Гущер",
				Breed = "Брадат дракон",
				Town = "Хасково",
				Description = "Търся друг гущер за приятел. Обичам да се пека на лампата и да ям скакалци. Махам си с ръка за поздрав.",
				PhoneNumber = "0871234567",
				ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQtyukiUUwuqFgAHnUDE9QkkiUetBo50Uwnzw&s",
				CategoryId = 7
			},

            new Animal
			{
				Id = 29,
				Name = "Бодльо",
				Species = "Таралеж",
				Breed = "Див",
				Town = "Ловеч",
				Description = "Дойдох сам в градината. Търся приятел за вечерни разходки. Ям кучешка храна и съм много сладък.",
				PhoneNumber = "0872345678",
				ImageUrl = "https://www.kakvoe.eu/wp-content/uploads/2020/06/%D1%82%D0%B0%D1%80%D0%B0%D0%BB%D0%B5%D0%B6.jpeg",
				CategoryId = 11
			},		
				
		};

		public void Configure(EntityTypeBuilder<Animal> builder)
		{
			builder.HasData(animals);
		}


	}
}
