

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
		public string Description { get; set; } = null!;

		[Required(ErrorMessage = "Телефонният номер е задължителен.")]
		[StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
		[RegularExpression(@"^\d{10}$", ErrorMessage = "Телефонният номер трябва да е 10 цифри.")]
		public string PhoneNumber { get; set; } = null!;

		[MaxLength(2048)]
		[Url]
		public string? ImageUrl { get; set; }

		public string? OwnerId { get; set; }

		[Range(-90, 90, ErrorMessage = "Географската ширина трябва да е между -90 и 90.")]
		public double? Latitude { get; set; }

		[Range(-180, 180, ErrorMessage = "Географската дължина трябва да е между -180 и 180.")]
		public double? Longitude { get; set; }



		/// <summary>
		/// Връзка с категорията 
		/// </summary>


		[Required]
		[ForeignKey(nameof(Category))]
		public int CategoryId { get; set; }
		public virtual Category? Category { get; set; }

		

	}
}
