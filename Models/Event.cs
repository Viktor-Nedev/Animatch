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

        public string? CreatedById { get; set; }
    }
}
