using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<Category?> GetCategoryAsync(int id);
        Task<List<Pokemon>> GetPokemonsByCategoryAsync(int categoryId);
        Task<bool> CategoryExistsAsync(int id);
        Task<bool> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(Category category);
        Task<bool> CategoryExistByNameAsync(string normalizedName);
    }
}