using Microsoft.EntityFrameworkCore;
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
        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _context.Categories
                                 .AsNoTracking()
                                 .AnyAsync(c => c.Id == id);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task<Category?> GetCategoryAsync(int id)
        {
            return await _context.Categories.AsNoTracking()
                                            .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Pokemon>> GetPokemonsByCategoryAsync(int categoryId)
        {
            return await _context.PokemonCategories
                           .AsNoTracking()
                           .Where(c => c.CategoryId == categoryId)
                           .Select(p  => p.Pokemon).ToListAsync();
        }

        public async Task<bool> CategoryExistByNameAsync(string normalizedName)
        {
            return await _context.Categories
                                 .AsNoTracking()
                                 .AnyAsync(c => c.Name.Trim().ToUpper() == normalizedName);
        }


        //POST Methods
        public async Task<bool> CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
           
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
           var saved = await _context.SaveChangesAsync();
           return saved > 0;
        }

        //PUT Methods
        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            return await SaveAsync();
        }


        //DELETE Methods
        public async Task<bool> DeleteCategoryAsync(Category category)
        {
            _context.Categories.Attach(category);
            _context.Categories.Remove(category);
            return await SaveAsync();
        }
    }
}