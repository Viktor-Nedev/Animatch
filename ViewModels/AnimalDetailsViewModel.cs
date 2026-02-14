namespace Animatch.ViewModels.Animal
{
    public class AnimalDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Species { get; set; } = null!;
        public string? Breed { get; set; }
        public string? Town { get; set; }
        public string Description { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
    }
}