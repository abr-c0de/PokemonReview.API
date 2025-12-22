namespace PokemonReviewApp.Dto
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public decimal Rating { get; set; }
    }
}
