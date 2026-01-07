using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext context;

        public ReviewRepository(DataContext context)
        {
            this.context = context;
        }

        //GET

        public async Task<Review?> GetReviewAsync(int reviewId)
        {
            return await context.Reviews
                                .AsNoTracking().FirstOrDefaultAsync(r => r.Id == reviewId);
        }

        public async Task<List<Review>> GetReviewsAsync()
        {
            return await context.Reviews
                                .AsNoTracking().ToListAsync();
        }

        public async Task<List<Review>> GetReviewsOfPokemonAsync(int pokeId)
        {
            return await context.Reviews
                                .AsNoTracking().Where(r => r.PokemonId  == pokeId).ToListAsync();
        }

        public async Task<bool> ReviewExistsAsync(int reviewId)
        {
            return await context.Reviews
                                .AsNoTracking().AnyAsync(r => r.Id == reviewId);
        }

        public async Task<bool> ReviewExistsByReviewerAsync(int pokemonId, int reviewerId)
        {

            return await context.Reviews
                                 .AsNoTracking()
                                 .AnyAsync(r => r.PokemonId == pokemonId && r.ReviewerId == reviewerId);
        }

        //POST
        public async Task<bool> CreateReviewAsync(Review review)
        {
            await context.Reviews.AddAsync(review);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await context.SaveChangesAsync();
            return saved > 0;
        }


        //PUT
        public async Task<bool> UpdateReviewAsync(Review review)
        {
            context.Reviews.Update(review);
            return await SaveAsync();

        }

        //DELETE 
        public async Task<bool> DeleteReviewAsync(Review review)
        {
            context.Reviews.Remove(review);
            return await SaveAsync();
        }
    }
}