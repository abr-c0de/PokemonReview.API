using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonCategoryRepository
    {
        bool AddPokemonCategory(PokemonCategory P_C);
        bool Exist(int PokemonId, int CategoryId);
        bool Save();
    }
}
