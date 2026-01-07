using PokemonReviewApp.Models;

namespace PokemonReviewApp.Dto
{
    public class OwnerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Gym { get; set; } = null!;
        public int CountryId { get; set; }
    }
}
