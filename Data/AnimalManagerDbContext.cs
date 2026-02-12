

namespace Animatch.Data
{
	using Animatch.Models;
	using Microsoft.EntityFrameworkCore;

	public class AnimalManagerDbContext : DbContext
	{

		public AnimalManagerDbContext(DbContextOptions<AnimalManagerDbContext> dbContextOptions)
			: base(dbContextOptions)
		{


		}


		public DbSet<Animal> Animals { get; set; } = null!;

		public DbSet<Category> Categories { get; set; } = null!;


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnimalManagerDbContext).Assembly);
		}

	}
}
