using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApp.Models
{
    public class Owner
    {
        public int Id { get; set; }
        [Required][MaxLength(100)]public string Name { get; set; } = null!;
        [Required][MaxLength(60)] public string Gym { get; set; } = null!;
        [Required]public int CountryId { get; set; }
        public Country Country { get; set; } = null!;
        public ICollection<PokemonOwners> PokemonOwners { get; set; } = new List<PokemonOwners>();
    }
}
