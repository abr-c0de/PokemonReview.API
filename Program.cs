using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PokemonReviewApp;
using PokemonReviewApp.Data;
using PokemonReviewApp.Helper;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repository;
using System.Threading.Tasks;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfiles>();
        });

        builder.Services.AddTransient<Seed>();

        builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<ICountryRepository, CountryRepository>();
        builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        builder.Services.AddScoped<IReviewerRepository, ReviewerRepository>();
        builder.Services.AddScoped<IPokemonCategoryRepository, PokemonCategoryRepository>();
        builder.Services.AddScoped<IPokemonOwnersRepository, PokemonOwnersRepository>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        var app = builder.Build();



        if (args.Length == 1 && args[0].ToLower() == "seeddata")
        {
            await SeedData(app);

            static async Task SeedData(IHost app)
            {
                var ScopedFactory = app.Services.GetService<IServiceScopeFactory>();

                using (var scope = ScopedFactory?.CreateScope())
                {
                    var service = scope?.ServiceProvider.GetService<Seed>();

                    if (service != null)
                    {
                        await service.SeedDataContextAsync();
                        Console.WriteLine("Seed completed successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Seed service is null. Check DI registration.");
                    }
                }
            }
        }


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}