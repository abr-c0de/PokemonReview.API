using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonOwnersRepository
    {
        //POST
        Task<bool> AddPokemonOwnerAsync(PokemonOwners P_O);
        Task<bool> ExistAsync(int PokemonId, int OwnerId);
    }
}
