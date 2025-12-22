namespace PokemonReviewApp.Models
{
    public class Reviewer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
