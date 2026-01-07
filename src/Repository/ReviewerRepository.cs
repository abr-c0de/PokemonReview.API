using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext context;
        public ReviewerRepository(DataContext context)
        {
            this.context = context;
        }

        //GET
        public async Task<Reviewer?> GetReviewerAsync(int reviewerId)
        {
            return await context.Reviewers
                                .AsNoTracking().FirstOrDefaultAsync(r => r.Id == reviewerId);
        }

        public async Task<List<Reviewer>> GetReviewersAsync()
        {
            return await context.Reviewers
                                .AsNoTracking().ToListAsync();
        }

        public async Task<List<Review>> GetReviewsByReviewerAsync(int reviewerId)
        {
            return await context.Reviews
                                .AsNoTracking().Where(r => r.ReviewerId == reviewerId).ToListAsync();
        }

        public async Task<bool> ReviewerExistsAsync(int reviewerId)
        {
            return await context.Reviewers
                                .AsNoTracking().AnyAsync(r => r.Id == reviewerId);
        }

        public async Task<bool> ReviewerExistByNameAsync(string normalizedLastName)
        {

            return await context.Reviewers
                                 .AsNoTracking()
                                 .AnyAsync(p => p.LastName != null && p.LastName.Trim().ToUpper() == normalizedLastName);
        }

        //POST
        public async Task<bool> CreateReviewerAsync(Reviewer reviewer)
        {
            await context.Reviewers.AddAsync(reviewer);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await context.SaveChangesAsync();
            return saved > 0;
        }

        //PUT
        public async Task<bool> UpdateReviewerAsync(Reviewer reviewer)
        {
            context.Reviewers.Update(reviewer);
            return await SaveAsync();

        }

        //DELETE 
        public async Task<bool> DeleteReviewerAsync(Reviewer reviewer)
        {
            context.Reviewers.Remove(reviewer);
            return await SaveAsync();
        }
    }

}
