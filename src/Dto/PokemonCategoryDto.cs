using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApp.Dto
{
    public class PokemonCategoryDto
    {
        [Required] public int PokemonId { get; set; }
        [Required] public int CategoryId { get; set; }
    }
}
