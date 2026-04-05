using Animatch.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Animatch.Data.Configuration
{
    public class WalkRequestEntityConfiguration : IEntityTypeConfiguration<WalkRequest>
    {
        private readonly IEnumerable<WalkRequest> walkRequests = new List<WalkRequest>
        {
            new WalkRequest
            {
                Id = 1,
                AnimalId = 1,
                RequesterId = "seed-user-1",
                Status = RequestStatus.Pending,
                Message = "Свободен съм в събота следобед за разходка.",
                RequestedOn = new DateTime(2026, 4, 1, 14, 0, 0, DateTimeKind.Utc)
            },
            new WalkRequest
            {
                Id = 2,
                AnimalId = 2,
                RequesterId = "seed-user-2",
                Status = RequestStatus.Approved,
                Message = "Мога да помогна в неделя сутрин.",
                RequestedOn = new DateTime(2026, 4, 2, 9, 30, 0, DateTimeKind.Utc)
            },
            new WalkRequest
            {
                Id = 3,
                AnimalId = 6,
                RequesterId = "seed-user-3",
                Status = RequestStatus.Rejected,
                Message = "Искам да участвам и в следващи инициативи.",
                RequestedOn = new DateTime(2026, 4, 3, 11, 15, 0, DateTimeKind.Utc)
            }
        };

        public void Configure(EntityTypeBuilder<WalkRequest> builder)
        {
            builder
                .HasOne(w => w.Animal)
                .WithMany(a => a.WalkRequests)
                .HasForeignKey(w => w.AnimalId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(walkRequests);
        }
    }
}
