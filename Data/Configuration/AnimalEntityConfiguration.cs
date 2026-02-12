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
				CategoryId = 11
			},		
				
		};

		public void Configure(EntityTypeBuilder<Animal> builder)
		{
			builder.HasData(animals);
		}


	}
}