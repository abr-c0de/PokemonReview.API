using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonOwnersRepository : IPokemonOwnersRepository
    {
        private readonly DataContext context;
        public PokemonOwnersRepository(DataContext context)
        {
            this.context = context;
        }


        public bool AddPokemonOwner(PokemonOwners P_O)
        {
            context.Add(P_O);
            return Save();
        }

        public bool Exist(int PokemonId, int OwnerId)
        {
            return context.PokemonOwners
                          .Any(po => po.PokemonId == PokemonId && po.OwnerId == OwnerId);
        }

        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved > 0;
        }
    }
}
