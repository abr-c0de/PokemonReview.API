using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonCategoryRepository : IPokemonCategoryRepository
    {
        private readonly DataContext context;
        public PokemonCategoryRepository(DataContext context) 
        {
            this.context = context;
        }
        public async Task<bool> AddPokemonCategoryAsync(PokemonCategory P_C)
        {
            await context.PokemonCategories.AddAsync(P_C);
            return await SaveAsync();
        }

        public async Task<bool> ExistAsync(int PokemonId, int CategoryId)
        {
            return await context.PokemonCategories
                                .AsNoTracking()
                                .AnyAsync(pc => pc.PokemonId == PokemonId && pc.CategoryId == CategoryId);
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await context.SaveChangesAsync();

            return saved > 0;
        }
    }
}
