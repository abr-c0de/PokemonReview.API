using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        Task<List<Pokemon>> GetPokemonsAsync();
        Task<Pokemon?> GetPokemonAsync(int id);
        Task<Pokemon?> GetPokemonByNameAsync(string name);
        Task<decimal> GetPokemonRatingAsync(int Pokeid);
        Task<bool> PokemonExistsAsync(int Pokeid);
        Task<bool> PokemonExistByNameAsync(string normalizedName);
        Task<bool> CreatePokemonAsync(Pokemon pokemon);
        Task<bool> UpdatePokemonAsync(Pokemon pokemon);
        Task<bool> DeletePokemonAsync(Pokemon pokemon);
    }
}