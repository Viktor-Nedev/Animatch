using Animatch.Models;

namespace Animatch.ViewModels
{
    public class MyWalkRequestsViewModel
    {
        public IEnumerable<WalkRequest> CreatedRequests { get; set; } = new List<WalkRequest>();
        public IEnumerable<WalkRequest> IncomingRequests { get; set; } = new List<WalkRequest>();
        public bool IsAdmin { get; set; }
    }
}
