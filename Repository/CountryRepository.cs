using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext context;

        public CountryRepository(DataContext context)
        {
            this.context = context;
        }

        //GET

        public async Task<bool> CountryExistAsync(int countryId)
        {
            return await context.Countries
                                .AsNoTracking().AnyAsync(c => c.Id == countryId);
        }

        public async Task<List<Country>> GetCountriesAsync()
        {
            return await context.Countries
                                .AsNoTracking().ToListAsync();
        }

        public async Task<Country?> GetCountryAsync(int id)
        {
            return await context.Countries
                                .AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Country?> GetCountryByOwnerAsync(int ownerId)
        {
            return await context.Owners
                          .AsNoTracking()
                          .Include(o => o.Country)
                          .Where(o => o.Id == ownerId)
                          .Select(o => o.Country)
                          .FirstOrDefaultAsync();
        }

        public async Task<List<Owner>> GetOwnersFromCountryAsync(int countryId)
        {
            return await context.Owners
                          .AsNoTracking()
                          .Where(o => o.CountryId == countryId)
                          .ToListAsync();
        }

        public async Task<bool> CountryExistByNameAsync(string normalizedName)
        {
            return await context.Countries
                                .AsNoTracking()
                                .AnyAsync(c => c.Name != null && c.Name.Trim().ToUpper() == normalizedName);
        }



        //POST
        public async Task<bool> CreateCountryAsync(Country country)
        {
            await context.Countries.AddAsync(country);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await context.SaveChangesAsync();
            return saved > 0;
        }


        //PUT
        public async Task<bool> UpdateCountryAsync(Country country)
        {
            context.Countries.Update(country);
            return await SaveAsync();
        }

        //DELETE 
        public async Task<bool> DeleteCountryAsync(Country country)
        {
            context.Countries.Attach(country);
            context.Countries.Remove(country);
            return await SaveAsync();
        }
    }
}
