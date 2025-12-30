using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApp.Models
{
    public class PokemonCategory
    {
        [Required]public int PokemonId { get; set; }
        [Required]public int CategoryId { get; set; }
        public Pokemon Pokemon { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
