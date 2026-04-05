using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animatch.Models
{
    public class WalkRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Animal))]
        public int AnimalId { get; set; }

        public Animal Animal { get; set; } = null!;

        [Required]
        [MaxLength(450)]
        public string RequesterId { get; set; } = null!;

        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        [MaxLength(500)]
        public string? Message { get; set; }

        public DateTime RequestedOn { get; set; } = DateTime.UtcNow;
    }
}
