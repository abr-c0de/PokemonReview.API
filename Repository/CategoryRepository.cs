using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context) 
        {
            _context = context;
        }


        //GET Methods
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public ICollection<Pokemon> GetPokemonsByCategory(int categoryId)
        {
            return _context.PokemonCategories
                           .Where(c => c.CategoryId == categoryId)
                           .Select(p  => p.Pokemon).ToList();
        }


        //POST Methods
        public bool CreateCategory(Category category)
        {
            _context.Add(category);
           
            return Save();
        }

        public bool Save()
        {
           var saved = _context.SaveChanges();
           return saved > 0;
        }

        //PUT Methods
        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }


        //DELETE Methods
        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }
    }
}