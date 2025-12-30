using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;
        public PokemonRepository(DataContext context)
        {
            this._context = context;
        }


        //GET
        public async Task<Pokemon?> GetPokemonAsync(int id)
        {
            return await _context.Pokemons
                                 .AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pokemon?> GetPokemonByNameAsync(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return await _context.Pokemons
                                 .AsNoTracking().FirstOrDefaultAsync(p => p.Name.Trim().ToUpper() == name);
        }

        public async Task<decimal> GetPokemonRatingAsync(int Pokeid)
        {
            var reviews = await _context.Reviews
                                       .AsNoTracking()
                                       .Where(p => p.PokemonId == Pokeid)
                                       .ToListAsync();

            if (!reviews.Any()) return 0;

            return ((decimal)reviews.Sum(r => r.Rating) / reviews.Count());
        }

        public async Task<List<Pokemon>> GetPokemonsAsync()
        {
            return await _context.Pokemons
                                 .AsNoTracking().ToListAsync();
        }

        public async Task<bool> PokemonExistsAsync(int Pokeid)
        {
            return await _context.Pokemons
                                 .AsNoTracking().AnyAsync(p => p.Id == Pokeid);
        }

        public async Task<bool> PokemonExistByNameAsync(string normalizedName)
        {

            return await _context.Pokemons
                                 .AsNoTracking()
                                 .AnyAsync(p => p.Name != null && p.Name.Trim().ToUpper() == normalizedName);
        }

        //POST
        public async Task<bool> CreatePokemonAsync(Pokemon pokemon)
        {
            await _context.Pokemons.AddAsync(pokemon);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        //PUT
        public async Task<bool> UpdatePokemonAsync(Pokemon pokemon)
        {
            _context.Pokemons.Update(pokemon);
            return await SaveAsync();
        }

        //DELETE 
        public async Task<bool> DeletePokemonAsync(Pokemon pokemon)
        {
            _context.Pokemons.Attach(pokemon);
            _context.Pokemons.Remove(pokemon);
            return await SaveAsync();
        }
    }
}