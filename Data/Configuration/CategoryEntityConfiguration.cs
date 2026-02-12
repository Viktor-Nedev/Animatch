namespace Animatch.Data.Configuration
{
	using Models;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
	{
		private IEnumerable<Category> categories = new List<Category>()
		{
			new Category { Id = 1, Name = "Кучета" },
			new Category { Id = 2, Name = "Котки" },
			new Category { Id = 3, Name = "Птици" },
			new Category { Id = 4, Name = "Зайци" },
			new Category { Id = 5, Name = "Гризачи" },
			new Category { Id = 6, Name = "Риби" },
			new Category { Id = 7, Name = "Влечуги" },
			new Category { Id = 8, Name = "Земноводни" },
			new Category { Id = 9, Name = "Конe" },
			new Category { Id = 10, Name = "Селскостопански" },
			new Category { Id = 11, Name = "Диви животни" }
		};

		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.HasData(categories);
		}

	}
}
