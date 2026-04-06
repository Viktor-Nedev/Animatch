using Animatch.Models;

namespace Animatch.ViewModels
{
    public class AnimalIndexQueryViewModel
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public string? Town { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 9;
        public int TotalCount { get; set; }
        public IEnumerable<Animal> Animals { get; set; } = new List<Animal>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<string> Towns { get; set; } = new List<string>();
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
