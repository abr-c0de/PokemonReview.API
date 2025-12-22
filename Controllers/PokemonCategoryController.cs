using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonCategoryController : ControllerBase
    {
        private readonly IPokemonCategoryRepository pokemonCategoryRepository;
        private readonly IPokemonRepository pokemonRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;


        public PokemonCategoryController(IPokemonCategoryRepository pokemonCategoryRepository, IMapper mapper,
                      IPokemonRepository pokemonRepository, ICategoryRepository categoryRepository)

        {
            this.pokemonCategoryRepository = pokemonCategoryRepository;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
            this.pokemonRepository = pokemonRepository;
        }

        //POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult AddPokemonCategory([FromBody] PokemonCategoryDto pokemonCategoryCreate)
        {
            if(!ModelState.IsValid)
               return BadRequest(ModelState);

            var pokemonExists = pokemonRepository.GetPokemon(pokemonCategoryCreate.PokemonId);
            var categoryExists = categoryRepository.GetCategory(pokemonCategoryCreate.CategoryId);

            if(pokemonExists == null || categoryExists == null ) return NotFound(new {message = "Pokemon or Category not found" });

            if(pokemonCategoryRepository.Exist(pokemonCategoryCreate.PokemonId, pokemonCategoryCreate.CategoryId))
            {
                return Conflict(new { message = "Relation already exists" });
            }

            var mappedPokemonCategory = mapper.Map<PokemonCategory>(pokemonCategoryCreate);

            if (!pokemonCategoryRepository.AddPokemonCategory(mappedPokemonCategory))
            {
                return StatusCode(500, new {message = "Failed to save" });
            }

            return Ok("pokemon linked to category successfully.");
        }
    }
}