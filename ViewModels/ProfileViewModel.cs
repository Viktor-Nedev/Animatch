using Animatch.Models;

namespace Animatch.ViewModels
{
    public class ProfileViewModel
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public IEnumerable<Animal> MyAnimals { get; set; } = new List<Animal>();
    }
}
