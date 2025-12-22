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
        public bool AddPokemonCategory(PokemonCategory P_C)
        {
            context.Add(P_C);
            return Save();
        }

        public bool Exist(int PokemonId, int CategoryId)
        {
            return context.PokemonCategories
                          .Any(pc => pc.PokemonId == PokemonId && pc.CategoryId == CategoryId);
        }

        public bool Save()
        {
            var saved = context.SaveChanges();

            return saved > 0;
        }
    }
}
