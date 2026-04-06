using Animatch.Models;

namespace Animatch.Services
{
    public interface IWalkRequestService
    {
        Task<WalkRequest?> GetByIdAsync(int id);
        Task<IEnumerable<WalkRequest>> GetByRequesterAsync(string requesterId);
        Task<IEnumerable<WalkRequest>> GetIncomingForOwnerAsync(string ownerId);
        Task<IEnumerable<WalkRequest>> GetAllAsync();
        Task<bool> HasPendingAsync(int animalId, string requesterId);
        Task AddAsync(WalkRequest request);
        Task UpdateAsync(WalkRequest request);
    }
}
