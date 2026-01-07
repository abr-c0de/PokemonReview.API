using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonCategoryRepository
    {
        Task<bool> AddPokemonCategoryAsync(PokemonCategory P_C);
        Task<bool> ExistAsync(int PokemonId, int CategoryId);
    }
}
