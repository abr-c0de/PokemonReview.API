namespace PokemonReviewApp.Dto
{
    public class OwnerUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Gym { get; set; } = null!;
        public int CountryId { get; set; }
    }
}
