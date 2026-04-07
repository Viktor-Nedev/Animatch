using System.ComponentModel.DataAnnotations;

namespace Animatch.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(2000, MinimumLength = 20)]
        public string Description { get; set; } = null!;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Location { get; set; } = null!;

        [Range(-90, 90, ErrorMessage = "Географската ширина трябва да е между -90 и 90.")]
        public double? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Географската дължина трябва да е между -180 и 180.")]
        public double? Longitude { get; set; }

        public string? CreatedById { get; set; }
    }
}
