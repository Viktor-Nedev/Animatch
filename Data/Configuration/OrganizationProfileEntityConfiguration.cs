using Animatch.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Animatch.Data.Configuration
{
    public class OrganizationProfileEntityConfiguration : IEntityTypeConfiguration<OrganizationProfile>
    {
        public void Configure(EntityTypeBuilder<OrganizationProfile> builder)
        {
            builder.HasIndex(x => x.UserId).IsUnique();

            builder.HasData(
                new OrganizationProfile
                {
                    Id = 1,
                    OrganizationName = "Happy Paws Foundation",
                    Description = "Доброволческа организация за разходки на кучета, временни домове и транспорт до клиники.",
                    Town = "Sofia",
                    PhoneNumber = "0888123456",
                    WebsiteUrl = "https://happypaws.example.org",
                    LogoUrl = "https://images.unsplash.com/photo-1601758228041-f3b2795255f1?auto=format&fit=crop&w=800&q=80",
                    UserId = "seed-organizer-1",
                    CreatedOn = new DateTime(2026, 1, 10, 9, 0, 0, DateTimeKind.Utc)
                },
                new OrganizationProfile
                {
                    Id = 2,
                    OrganizationName = "Stray Care Collective",
                    Description = "Екип, който координира кампании за хранене, кастрация и социализация на бездомни животни.",
                    Town = "Plovdiv",
                    PhoneNumber = "0899765432",
                    WebsiteUrl = "https://straycare.example.org",
                    LogoUrl = "https://images.unsplash.com/photo-1554692936-19b9b7f0f89a?auto=format&fit=crop&w=800&q=80",
                    UserId = "seed-organizer-2",
                    CreatedOn = new DateTime(2026, 1, 15, 11, 30, 0, DateTimeKind.Utc)
                });
        }
    }
}
