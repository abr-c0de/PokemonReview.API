using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Required][MaxLength(76)]public string Title { get; set; } = null!;
        [Required][MaxLength(600)] public string Text { get; set; } = null!;
        [Required][Column(TypeName = "decimal(3,1)")]public decimal Rating { get; set; }
        [Required]public int ReviewerId { get; set; }
        [Required]public int PokemonId { get; set; }
        public Reviewer Reviewer { get; set; } = null!;
        public Pokemon Pokemon { get; set; } = null!;

    }
}