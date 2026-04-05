using System.ComponentModel.DataAnnotations;

namespace Animatch.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;
        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = null!;
        [Required]
        public DateTime Date { get; set; }
        [MaxLength(200)]
        public string Location { get; set; } = null!;
        public string? CreatedById { get; set; }
    }
}
