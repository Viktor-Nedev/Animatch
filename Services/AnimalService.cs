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

        public async Task<IEnumerable<Animal>> GetAllWithCategoryAsync()
        {
            return await context.Animals.Include(a => a.Category).AsNoTracking().ToListAsync();
        }

        public async Task<(IEnumerable<Animal> Items, int TotalCount)> GetPagedFilteredAsync(string? searchTerm, int? categoryId, string? town, int page, int pageSize)
        {
            var query = context.Animals
                .Include(a => a.Category)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalized = searchTerm.Trim().ToLower();
                query = query.Where(a => a.Name.ToLower().Contains(normalized));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(a => a.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(town))
            {
                var normalizedTown = town.Trim().ToLower();
                query = query.Where(a => a.Town != null && a.Town.ToLower() == normalizedTown);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(a => a.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<Animal>> GetWithCoordinatesAsync()
        {
            return await context.Animals
                .Where(a => a.Latitude.HasValue && a.Longitude.HasValue)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Animal>> GetByOwnerAsync(string ownerId)
        {
            return await context.Animals
                .Where(a => a.OwnerId == ownerId)
                .Include(a => a.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetDistinctTownsAsync()
        {
            return await context.Animals
                .Select(a => a.Town)
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => t!)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();
        }

        public async Task<Animal?> GetByIdWithCategoryAsync(int id)
        {
            return await context.Animals.Include(a => a.Category).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Animals.AnyAsync(e => e.Id == id);
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
