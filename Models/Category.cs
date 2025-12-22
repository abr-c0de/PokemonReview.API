namespace PokemonReviewApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<PokemonCategory> PokemonCategories { get; set; } = new List<PokemonCategory>();
    }
}
