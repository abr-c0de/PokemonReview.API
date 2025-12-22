namespace PokemonReviewApp.Dto
{
    public class ReviewUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public decimal Rating { get; set; }
        public int ReviewerId { get; set; }
        public int PokemonId { get; set; }
    }
}
