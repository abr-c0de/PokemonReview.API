using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            this.mapper = mapper;
        }

        //GET

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PokemonDto>>> GetPokemons()
        {
            var pokemons = mapper.Map<List<PokemonDto>>(await _pokemonRepository.GetPokemonsAsync());

            if (!pokemons.Any())
                return Ok(new List<PokemonDto>());

            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<PokemonDto>> GetPokemon(int pokeId)
        {
            var pokemonDb = await _pokemonRepository.GetPokemonAsync(pokeId);

            if (pokemonDb == null)
                return NotFound();

            var pokemon = mapper.Map<PokemonDto>(pokemonDb);

            return Ok(pokemon);

        }

        [HttpGet("{name}/byName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        public async Task<ActionResult<PokemonDto>> GetPokemonByName(string name)
        {
            if (name == null)
                return NotFound();

            var pokemonEntity = await _pokemonRepository.GetPokemonByNameAsync(name.Trim().ToUpper());

            if (pokemonEntity == null)
                return NotFound();

            var poke = mapper.Map<PokemonDto>(pokemonEntity);

            return Ok(poke);
        }


        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<Pokemon>> GetPokemonRating(int pokeId)
        {
            var pokemon = await _pokemonRepository.GetPokemonAsync(pokeId);

            if (pokemon == null)
                return NotFound();

            var rating = await _pokemonRepository.GetPokemonRatingAsync(pokeId); 

            return Ok(rating);
        }


        //POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePokemon([FromBody] PokemonCreateDto pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest("Pokemon data is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(pokemonCreate.Name))
                return BadRequest("Pokemon name is required.");

            var existingPokemon = await _pokemonRepository.PokemonExistByNameAsync(pokemonCreate.Name.Trim().ToUpper());

            if (existingPokemon)
                return Conflict(" Pokemon already exists. ");

            var mappedPokemon = mapper.Map<Pokemon>(pokemonCreate);

            var created = await _pokemonRepository.CreatePokemonAsync(mappedPokemon);

            if (!created)
                return StatusCode(500, "Something went wrong while saving. ");

            var responseDto = mapper.Map<PokemonDto>(mappedPokemon);

            return CreatedAtAction(
                          nameof(GetPokemon),
                          new { pokeId = mappedPokemon.Id },
                          responseDto);
        }


        //PUT
        [HttpPut("{pokeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePokemon(int pokeId, [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
                return BadRequest("Pokemon data is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (pokeId != updatedPokemon.Id)
                return BadRequest("Route ID and body ID do not match");

            if (!await _pokemonRepository.PokemonExistsAsync(pokeId))
                return NotFound();

            var mappedPokemon = mapper.Map<Pokemon>(updatedPokemon);

            if (!await _pokemonRepository.UpdatePokemonAsync(mappedPokemon)) 
                return StatusCode(500, "Something went wrong while saving");

            return NoContent();
        }

        //DELETE
        [HttpDelete("{pokeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePokemon(int pokeId)
        {

            var pokemonToDelete = await _pokemonRepository.GetPokemonAsync(pokeId);

            if (pokemonToDelete == null)
                return NotFound();

            if (!await _pokemonRepository.DeletePokemonAsync(pokemonToDelete))
                return StatusCode(500, "Something went wrong while deleting");

            return NoContent();
        }

    }
}
