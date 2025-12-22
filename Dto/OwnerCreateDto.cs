namespace PokemonReviewApp.Dto
{
    public class OwnerCreateDto
    {
        public int CountryId { get; set; }
        public string Name { get; set; } = null!;
        public string Gym { get; set; } = null!;
    }
}
