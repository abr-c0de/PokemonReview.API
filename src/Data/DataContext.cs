using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<PokemonOwners> PokemonOwners { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PokemonCategory>()
                        .HasKey(pc => new { pc.PokemonId, pc.CategoryId });

            modelBuilder.Entity<PokemonCategory>()
                        .HasOne(p => p.Pokemon)
                        .WithMany(pc => pc.PokemonCategories)
                        .HasForeignKey(p => p.PokemonId)
                        .OnDelete(DeleteBehavior.Restrict); // prevent cascade delete

            modelBuilder.Entity<PokemonCategory>()
                        .HasOne(c => c.Category)
                        .WithMany(pc => pc.PokemonCategories)
                        .HasForeignKey(p => p.CategoryId)
                        .OnDelete(DeleteBehavior.Restrict); //prevent cascade delete



            modelBuilder.Entity<PokemonOwners>()
                        .HasKey(po => new { po.PokemonId, po.OwnerId });

            modelBuilder.Entity<PokemonOwners>()
                        .HasOne(p => p.Pokemon)
                        .WithMany(po => po.PokemonOwners)
                        .HasForeignKey(p => p.PokemonId)
                        .OnDelete(DeleteBehavior.Restrict); //prevent cascade delete

            modelBuilder.Entity<PokemonOwners>()
                        .HasOne(o => o.Owner)
                        .WithMany(po => po.PokemonOwners)
                        .HasForeignKey(p => p.OwnerId)
                        .OnDelete(DeleteBehavior.Restrict); //prevent cascade delete



            modelBuilder.Entity<Review>()
                        .HasIndex(r => new { r.PokemonId, r.ReviewerId })
                        .IsUnique();

            modelBuilder.Entity<Review>()
                        .HasOne(r => r.Pokemon)
                        .WithMany(p => p.Reviews)
                        .HasForeignKey(r => r.PokemonId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                        .HasOne(r => r.Reviewer)
                        .WithMany(rv => rv.Reviews)
                        .HasForeignKey(r => r.ReviewerId)
                        .OnDelete(DeleteBehavior.Restrict);

        }    
    }
}
