using Animatch.Models;

namespace Animatch.Services
{
    public interface IAnimalService
    {
        Task<IEnumerable<Animal>> GetAllWithCategoryAsync();
        Task<(IEnumerable<Animal> Items, int TotalCount)> GetPagedFilteredAsync(string? searchTerm, int? categoryId, string? town, int page, int pageSize);
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
