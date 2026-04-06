using Animatch.Models;

namespace Animatch.ViewModels
{
    public class EventIndexQueryViewModel
    {
        public string? SearchTerm { get; set; }
        public string? Location { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 6;
        public int TotalCount { get; set; }
        public IEnumerable<Event> Events { get; set; } = new List<Event>();
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
