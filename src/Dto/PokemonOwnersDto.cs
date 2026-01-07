using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApp.Dto
{
    public class PokemonOwnersDto
    {
        [Required] public int PokemonId { get; set; }
        [Required] public int OwnerId { get; set; }
    }
}
