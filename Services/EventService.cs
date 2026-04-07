using Animatch.Data;
using Animatch.Models;
using Microsoft.EntityFrameworkCore;

namespace Animatch.Services
{
    public class EventService : IEventService
    {
        private readonly AnimalManagerDbContext context;

        public EventService(AnimalManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await context.Set<Event>()
                .AsNoTracking()
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetWithCoordinatesAsync()
        {
            return await context.Set<Event>()
                .AsNoTracking()
                .Where(e => e.Latitude.HasValue && e.Longitude.HasValue)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetByCreatorAsync(string creatorId)
        {
            return await context.Set<Event>()
                .AsNoTracking()
                .Where(e => e.CreatedById == creatorId)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Event> Items, int TotalCount)> GetPagedAsync(string? searchTerm, string? location, int page, int pageSize)
        {
            var query = context.Set<Event>()
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalized = searchTerm.Trim().ToLower();
                query = query.Where(e => e.Title.ToLower().Contains(normalized));
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                var normalizedLocation = location.Trim().ToLower();
                query = query.Where(e => e.Location.ToLower().Contains(normalizedLocation));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(e => e.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await context.Set<Event>().FindAsync(id);
        }

        public async Task AddAsync(Event ev)
        {
            await context.Set<Event>().AddAsync(ev);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Event ev)
        {
            context.Set<Event>().Update(ev);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await context.Set<Event>().FindAsync(id);
            if (entity != null)
            {
                context.Set<Event>().Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}
