using Animatch.Models;

namespace Animatch.Services
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<(IEnumerable<Event> Items, int TotalCount)> GetPagedAsync(string? searchTerm, string? location, int page, int pageSize);
        Task<Event?> GetByIdAsync(int id);
        Task AddAsync(Event ev);
        Task UpdateAsync(Event ev);
        Task DeleteAsync(int id);
    }
}
