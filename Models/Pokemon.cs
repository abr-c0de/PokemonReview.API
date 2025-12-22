namespace PokemonReviewApp.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public ICollection<Review> Reviews { get; set; }  = new List<Review>();
        public ICollection<PokemonOwners> PokemonOwners { get; set; } = new List<PokemonOwners>();
        public ICollection<PokemonCategory> PokemonCategories { get; set; } = new List<PokemonCategory>();

    }

}