using Animatch.Models;

namespace Animatch.Services
{
    public interface IAnimalService
    {
        Task<IEnumerable<Animal>> GetAllWithCategoryAsync();
        Task<IEnumerable<Animal>> GetWithCoordinatesAsync();
        Task<IEnumerable<Animal>> GetByOwnerAsync(string ownerId);
        Task<IEnumerable<string>> GetDistinctTownsAsync();
        Task<Animal?> GetByIdWithCategoryAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task AddAsync(Animal animal);
        Task UpdateAsync(Animal animal);
        Task DeleteAsync(int id);
    }
}
