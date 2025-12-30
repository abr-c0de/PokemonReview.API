using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApp.Models
{
    public class PokemonOwners
    {
        [Required]public int PokemonId { get; set; }
        [Required]public int OwnerId { get; set; }
        public Pokemon Pokemon { get; set; } = null!;
        public Owner Owner { get; set; } = null!;
    }
}
