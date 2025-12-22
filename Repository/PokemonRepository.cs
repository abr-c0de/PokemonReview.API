using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

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
        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemons.FirstOrDefault(p => p.Id == id);
        }

        public Pokemon GetPokemonByName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return _context.Pokemons.FirstOrDefault(p => p.Name == name);
        }

        public decimal GetPokemonRating(int Pokeid)
        {
            var review = _context.Reviews
                                 .Where(p => p.PokemonId == Pokeid);

            if(review.Count() <= 0) return 0;

            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.ToList();
        }

        public bool PokemonExists(int Pokeid)
        {
            return _context.Pokemons.Any(p => p.Id == Pokeid);
        }

        //POST
        public bool CreatePokemon(Pokemon pokemon)
        {
            _context.Add(pokemon);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        //PUT
        public bool UpdatePokemon(Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }

        //DELETE 
        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return Save();
        }
    }
}