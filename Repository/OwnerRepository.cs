using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Diagnostics.Metrics;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        
            private readonly DataContext context;
            public OwnerRepository(DataContext context)
            {
                this.context = context;
            }

        //GET

        public async Task<Owner?> GetOwnerAsync(int ownerId)
        {
            return await context.Owners
                                .AsNoTracking().FirstOrDefaultAsync(o => o.Id == ownerId);
        }

        public async Task<List<Owner>> GetOwnerOfPokemonAsync(int pokeId)
        {
            return await context.PokemonOwners
                          .AsNoTracking()
                          .Where(p => p.PokemonId == pokeId)
                          .Select(o => o.Owner)
                          .ToListAsync();
        }

        public async Task<List<Owner>> GetOwnersAsync()
        {
            return await context.Owners
                                .AsNoTracking().ToListAsync();
        }

        public async Task<List<Pokemon>> GetPokemonByOwnerAsync(int ownerId)
        {
            return await context.PokemonOwners
                          .AsNoTracking()
                          .Where(o => o.OwnerId == ownerId)
                          .Select(p =>  p.Pokemon)
                          .ToListAsync();
        }

        public async Task<bool> OwnerExistAsync(int ownerId)
        {
            return await context.Owners
                                .AsNoTracking().AnyAsync(o => o.Id == ownerId);
        }

        public async Task<bool> OwnerExistByNameAsync(string normalizedName)
        {
            return await context.Owners
                                .AsNoTracking()
                                .AnyAsync(o => o.Name != null && o.Name.Trim().ToUpper() == normalizedName);
        }


        //POST
        public async Task<bool> CreateOwnerAsync(Owner owner)
        {
            await context.Owners.AddAsync(owner);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await context.SaveChangesAsync();
            return saved > 0;
        }


        //PUT
        public async Task<bool> UpdateOwnerAsync(Owner owner)
        {
            context.Owners.Update(owner);
            return await SaveAsync();
        }

        //DELETE 
        public async Task<bool> DeleteOwnerAsync(Owner owner)
        {
            context.Owners.Attach(owner);
            context.Owners.Remove(owner);
            return await SaveAsync();
        }
    }
}
