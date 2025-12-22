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
        public Reviewer GetReviewer(int reviewerId)
        {
            return context.Reviewers.FirstOrDefault(r => r.Id == reviewerId);
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return context.Reviewers.ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return context.Reviews.Where(r => r.ReviewerId == reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return context.Reviewers.Any(r => r.Id == reviewerId);
        }

        //POST
        public bool CreateReviewer(Reviewer reviewer)
        {
            context.Add(reviewer);
            return Save();
        }

        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved > 0;
        }

        //PUT
        public bool UpdateReviewer(Reviewer reviewer)
        {
            context.Update(reviewer);
            return Save();

        }

        //DELETE 
        public bool DeleteReviewer(Reviewer reviewer)
        {
            context.Remove(reviewer);
            return Save();
        }
    }

}
