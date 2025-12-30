using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using Microsoft.EntityFrameworkCore;

namespace PokemonReviewApp
{
    public class Seed
    {
        private readonly DataContext _context;

        public Seed(DataContext context)
        {
            _context = context;
        }

        public async Task SeedDataContextAsync()
        {
            // Exit if data already exists
            if (await _context.PokemonOwners.AnyAsync())
                return;

            // 1️ Shared Categories
            var electricCategory = new Category { Name = "Electric" };
            var waterCategory = new Category { Name = "Water" };
            var leafCategory = new Category { Name = "Leaf" };

            // 2️ Shared Countries
            var kanto = new Country { Name = "Kanto" };
            var saffron = new Country { Name = "Saffron City" };
            var millet = new Country { Name = "Millet Town" };

            // 3️ Shared Reviewers
            var teddy = new Reviewer { FirstName = "Teddy", LastName = "Smith" };
            var taylor = new Reviewer { FirstName = "Taylor", LastName = "Jones" };
            var jessica = new Reviewer { FirstName = "Jessica", LastName = "McGregor" };

            // 4️ Owners
            var jack = new Owner { Name = "Jack", Gym = "Brocks Gym", Country = kanto };
            var harry = new Owner { Name = "Harry", Gym = "Mistys Gym", Country = saffron };
            var ash = new Owner { Name = "Ash", Gym = "Ashs Gym", Country = millet };

            // 5️ Pokemons
            var pikachu = new Pokemon
            {
                Name = "Pikachu",
                BirthDate = new DateTime(1903, 1, 1),
                PokemonCategories = new List<PokemonCategory> { new PokemonCategory { Category = electricCategory } },
                Reviews = new List<Review>
                {
                    new Review { Title = "Pikachu Review 1", Text = "Pikachu is electric and amazing", Reviewer = teddy },
                    new Review { Title = "Pikachu Review 2", Text = "Pikachu is fast and cute", Reviewer = taylor },
                    new Review { Title = "Pikachu Review 3", Text = "Electric type is the best", Reviewer = jessica }
                }
            };

            var squirtle = new Pokemon
            {
                Name = "Squirtle",
                BirthDate = new DateTime(1903, 1, 1),
                PokemonCategories = new List<PokemonCategory> { new PokemonCategory { Category = waterCategory } },
                Reviews = new List<Review>
                {
                    new Review { Title = "Squirtle Review 1", Text = "Squirtle is water type", Reviewer = teddy },
                    new Review { Title = "Squirtle Review 2", Text = "Squirtle is small but strong", Reviewer = taylor },
                    new Review { Title = "Squirtle Review 3", Text = "Squirtle, squirtle!", Reviewer = jessica }
                }
            };

            var venasuar = new Pokemon
            {
                Name = "Venasuar",
                BirthDate = new DateTime(1903, 1, 1),
                PokemonCategories = new List<PokemonCategory> { new PokemonCategory { Category = leafCategory } },
                Reviews = new List<Review>
                {
                    new Review { Title = "Venasuar Review 1", Text = "Venasuar is leaf type", Reviewer = teddy },
                    new Review { Title = "Venasuar Review 2", Text = "Strong against water types", Reviewer = taylor },
                    new Review { Title = "Venasuar Review 3", Text = "Venasuar, Venasuar!", Reviewer = jessica }
                }
            };

            // 6️ PokemonOwners
            var pokemonOwners = new List<PokemonOwners>
            {
                new PokemonOwners { Pokemon = pikachu, Owner = jack },
                new PokemonOwners { Pokemon = squirtle, Owner = harry },
                new PokemonOwners { Pokemon = venasuar, Owner = ash }
            };

            // 7️ Add everything at once
            await _context.Categories.AddRangeAsync(electricCategory, waterCategory, leafCategory);
            await _context.Countries.AddRangeAsync(kanto, saffron, millet);
            await _context.Reviewers.AddRangeAsync(teddy, taylor, jessica);
            await _context.Owners.AddRangeAsync(jack, harry, ash);
            await _context.Pokemons.AddRangeAsync(pikachu, squirtle, venasuar);
            await _context.PokemonOwners.AddRangeAsync(pokemonOwners);

            // 8️ Save changes once
            await _context.SaveChangesAsync();
        }
    }
}
