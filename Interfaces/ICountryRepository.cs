using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetCountriesAsync();    
        Task<Country?> GetCountryAsync(int id);
        Task<Country?> GetCountryByOwnerAsync(int ownerId);
        Task<List<Owner>> GetOwnersFromCountryAsync(int countryId);
        Task<bool> CountryExistAsync(int  countryId);
        Task<bool> CountryExistByNameAsync(string normalizedName);
        Task<bool> CreateCountryAsync(Country country);
        Task<bool> UpdateCountryAsync(Country country);
        Task<bool> DeleteCountryAsync(Country country);
    }
}
