
using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApp.Models
{
    public class Country
    {
        public int Id { get; set; }
        [Required][MaxLength(100)]public string Name { get; set; } = null!;
        public ICollection<Owner> Owners { get; set; } = new List<Owner>();
    }
}
