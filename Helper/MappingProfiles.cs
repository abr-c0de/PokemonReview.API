using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<PokemonDto, Pokemon>();
            CreateMap<PokemonCreateDto, Pokemon>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<CategoryCreateDto, Category>();

            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>();
            CreateMap<CountryCreateDto, Country>();

            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerDto, Owner>();
            CreateMap<OwnerCreateDto, Owner>();
            

            CreateMap<Review, ReviewDto>();
            CreateMap<ReviewDto, Review>();
            CreateMap<ReviewCreateDto, Review>();
            

            CreateMap<Reviewer, ReviewerDto>();
            CreateMap<ReviewerDto, Reviewer>();
            CreateMap<ReviewerCreateDto, Reviewer>();

            CreateMap<PokemonCategoryDto, PokemonCategory>();
            CreateMap<PokemonOwnersDto, PokemonOwners>();
        }
    }
}
