using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        
            private readonly DataContext context;
            public OwnerRepository(DataContext context)
            {
                this.context = context;
            }

        //GET

        public Owner GetOwner(int ownerId)
        {
            return context.Owners.FirstOrDefault(o => o.Id == ownerId);
        }

        public ICollection<Owner> GetOwnerOfPokemon(int pokeId)
        {
            return context.PokemonOwners
                          .Where(p => p.PokemonId == pokeId)
                          .Select(o => o.Owner)
                          .ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return context.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return context.PokemonOwners
                          .Where(o => o.OwnerId == ownerId)
                          .Select(p =>  p.Pokemon)
                          .ToList();
        }

        public bool OwnerExist(int ownerId)
        {
            return context.Owners.Any(o => o.Id == ownerId);
        }


        //POST
        public bool CreateOwner(Owner owner)
        {
            context.Add(owner);
            return Save();
        }

        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved > 0;
        }


        //PUT
        public bool UpdateOwner(Owner owner)
        {
            context.Update(owner);
            return Save();
        }

        //DELETE 
        public bool DeleteOwner(Owner owner)
        {
            context.Remove(owner);
            return Save();
        }
    }
}
