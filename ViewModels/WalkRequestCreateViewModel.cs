using System.ComponentModel.DataAnnotations;

namespace Animatch.ViewModels
{
    public class WalkRequestCreateViewModel
    {
        [Required]
        public int AnimalId { get; set; }

        public string AnimalName { get; set; } = string.Empty;
        public string AnimalTown { get; set; } = string.Empty;
        public string AnimalCategory { get; set; } = string.Empty;

        [StringLength(500, MinimumLength = 5)]
        public string? Message { get; set; }
    }
}
