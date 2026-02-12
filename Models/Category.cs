
namespace Animatch.Models
{
	using System.ComponentModel.DataAnnotations;
	
	using static Common.DataValidation.Category;
	public class Category
	{
			[Key]
			public int Id { get; set; }
			
			[Required]
			[MaxLength(NameMaxLength)]
			public string Name { get; set; } = null!;

			public virtual ICollection<Animal> Animals { get; set; }
			= new List<Animal>();

	}
}
