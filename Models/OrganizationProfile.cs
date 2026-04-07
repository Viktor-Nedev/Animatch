using System.ComponentModel.DataAnnotations;

using static Animatch.Common.DataValidation.OrganizationProfile;

namespace Animatch.Models
{
    public class OrganizationProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(OrganizationNameMaxLength, MinimumLength = OrganizationNameMinLength)]
        public string OrganizationName { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        [StringLength(TownMaxLength, MinimumLength = TownMinLength)]
        public string Town { get; set; } = null!;

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(2048)]
        [Url]
        public string? WebsiteUrl { get; set; }

        [MaxLength(2048)]
        [Url]
        public string? LogoUrl { get; set; }

        [Required]
        [MaxLength(450)]
        public string UserId { get; set; } = null!;

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
