using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApp.Models
{
    public class Reviewer
    {
        public int Id { get; set; }
        [Required][MaxLength(100)]public string FirstName { get; set; } = null!;
        [Required][MaxLength(100)]public string LastName { get; set; } = null!;
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
