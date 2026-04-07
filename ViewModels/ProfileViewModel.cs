using Animatch.Models;

namespace Animatch.ViewModels
{
    public class ProfileViewModel
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = "User";
        public bool CanCreateEvents { get; set; }
        public IEnumerable<Animal> MyAnimals { get; set; } = new List<Animal>();
        public IEnumerable<Event> MyEvents { get; set; } = new List<Event>();
    }
}
