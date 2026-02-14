

namespace Animatch.Models
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	using static Common.DataValidation.Animal;


	public class Animal
	{

		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; } = null!;


		[Required]
		[MaxLength(SpeciesMaxLength)]
		public string Species { get; set; } = null!;



		[MaxLength(BreedMaxLength)]
		public string? Breed { get; set; }



		[MaxLength(TownMaxLength)]
		public string? Town { get; set; }



		[Required]
		[MinLength(DescriptionMinLength)]
		[MaxLength(DescriptionMaxLength)]
		public string Description { get; set; }



		/// <summary>
		/// Връзка с категорията 
		/// </summary>


		[Required]
		[ForeignKey(nameof(Category))]
		public int CategoryId { get; set; }
		public virtual Category Category { get; set; } = null!;

		

	}
}
