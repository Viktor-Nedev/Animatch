using Animatch.Data;
using Animatch.Models;
using Microsoft.EntityFrameworkCore;

namespace Animatch.Services
{
    public class WalkRequestService : IWalkRequestService
    {
        private readonly AnimalManagerDbContext context;

        public WalkRequestService(AnimalManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<WalkRequest?> GetByIdAsync(int id)
        {
            return await context.WalkRequests
                .Include(r => r.Animal)
                .ThenInclude(a => a!.Category)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<WalkRequest>> GetByRequesterAsync(string requesterId)
        {
            return await context.WalkRequests
                .Include(r => r.Animal)
                .ThenInclude(a => a!.Category)
                .Where(r => r.RequesterId == requesterId)
                .OrderByDescending(r => r.RequestedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<WalkRequest>> GetIncomingForOwnerAsync(string ownerId)
        {
            return await context.WalkRequests
                .Include(r => r.Animal)
                .ThenInclude(a => a!.Category)
                .Where(r => r.Animal.OwnerId == ownerId)
                .OrderByDescending(r => r.RequestedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<WalkRequest>> GetAllAsync()
        {
            return await context.WalkRequests
                .Include(r => r.Animal)
                .ThenInclude(a => a!.Category)
                .OrderByDescending(r => r.RequestedOn)
                .ToListAsync();
        }

        public async Task<bool> HasPendingAsync(int animalId, string requesterId)
        {
            return await context.WalkRequests.AnyAsync(r =>
                r.AnimalId == animalId &&
                r.RequesterId == requesterId &&
                r.Status == RequestStatus.Pending);
        }

        public async Task AddAsync(WalkRequest request)
        {
            await context.WalkRequests.AddAsync(request);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WalkRequest request)
        {
            context.WalkRequests.Update(request);
            await context.SaveChangesAsync();
        }
    }
}
