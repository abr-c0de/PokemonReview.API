using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviewsAsync();
        Task<Review?> GetReviewAsync(int reviewId);
        Task<List<Review>> GetReviewsOfPokemonAsync(int pokeId);
        Task<bool> ReviewExistsAsync(int reviewId);
        Task<bool> ReviewExistsByReviewerAsync(int pokemonId, int reviewerId);
        Task<bool> CreateReviewAsync(Review review);
        Task<bool> UpdateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(Review review);
    }
}