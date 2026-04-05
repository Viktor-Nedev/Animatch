using Animatch.Data;
using Animatch.Models;
using Microsoft.EntityFrameworkCore;

namespace Animatch.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly AnimalManagerDbContext context;

        public AnimalService(AnimalManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Animal>> GetAllAsync()
        {
            return await context.Animals.Include(a => a.Category).AsNoTracking().ToListAsync();
        }

        public async Task<Animal?> GetByIdAsync(int id)
        {
            return await context.Animals.Include(a => a.Category).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Animal animal)
        {
            await context.Animals.AddAsync(animal);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Animal animal)
        {
            context.Animals.Update(animal);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await context.Animals.FindAsync(id);
            if (entity != null)
            {
                context.Animals.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}
