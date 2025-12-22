namespace PokemonReviewApp.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Gym { get; set; } = null!;
        public int CountryId { get; set; }
        public Country Country { get; set; } = null!;
        public ICollection<PokemonOwners> PokemonOwners { get; set; } = new List<PokemonOwners>();
    }
}
