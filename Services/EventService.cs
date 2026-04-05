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
            return await context.Set<Event>().AsNoTracking().ToListAsync();
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
