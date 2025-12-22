using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonOwnersRepository
    {
        //POST
        bool AddPokemonOwner(PokemonOwners P_O);
        bool Exist(int PokemonId, int OwnerId);
        bool Save();
    }
}
