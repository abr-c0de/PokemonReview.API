using Microsoft.EntityFrameworkCore;
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


        public async Task<bool> AddPokemonOwnerAsync(PokemonOwners P_O)
        {
            await context.PokemonOwners.AddAsync(P_O);
            return await SaveAsync();
        }

        public async Task<bool> ExistAsync(int PokemonId, int OwnerId)
        {
            return await context.PokemonOwners
                                .AsNoTracking()
                                .AnyAsync(po => po.PokemonId == PokemonId && po.OwnerId == OwnerId);
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await context.SaveChangesAsync();
            return saved > 0;
        }
    }
}
