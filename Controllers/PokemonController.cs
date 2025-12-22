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
        private readonly IPokemonRepository _PokemonRepository;
        private readonly IMapper mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _PokemonRepository = pokemonRepository;
            this.mapper = mapper;
        }

        //GET

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = mapper.Map<List<PokemonDto>>(_PokemonRepository.GetPokemons());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<PokemonDto> GetPokemon(int pokeId)
        {
            if (!_PokemonRepository.PokemonExists(pokeId)) return NotFound();

            var pokemon = mapper.Map<PokemonDto>(_PokemonRepository.GetPokemon(pokeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);

        }

        [HttpGet("{name}/byName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        public ActionResult<PokemonDto> GetPokemonByName(string name)
        {
            var pokemonEntity = _PokemonRepository.GetPokemonByName(name);

            if (pokemonEntity == null) return NotFound();

            var poke = mapper.Map<PokemonDto>(pokemonEntity);

            return Ok(poke);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<Pokemon> GetPokemonRating(int pokeId)
        {
            if (!_PokemonRepository.PokemonExists(pokeId)) return NotFound();

            var rating = _PokemonRepository.GetPokemonRating(pokeId);

            if(!ModelState.IsValid) 
                 return BadRequest(ModelState);

            return Ok(rating);
        }

        //POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreatePokemon([FromBody] PokemonDto pokemonCreate)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var normalizedName = pokemonCreate.Name.Trim().ToUpper();

            var existingPokemon = _PokemonRepository.GetPokemons()
                                               .FirstOrDefault(p => p.Name.ToUpper() == normalizedName);

            if (existingPokemon != null) return Conflict(" Pokemon already exists. ");

            var mappedPokemon = mapper.Map<Pokemon>(pokemonCreate);

            var created = _PokemonRepository.CreatePokemon(mappedPokemon);

            if (!created) return StatusCode(500, "Something went wrong while saving. ");

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
        public IActionResult UpdatePokemon(int pokeId, [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
                return BadRequest("Pokemon data is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (pokeId != updatedPokemon.Id)
                return BadRequest("Route ID and body ID do not match");

            if (!_PokemonRepository.PokemonExists(pokeId))
                return NotFound();
            var mappedPokemon = mapper.Map<Pokemon>(updatedPokemon);

            if (!_PokemonRepository.UpdatePokemon(mappedPokemon)) return StatusCode(500, "Something went wrong while saving");

            return NoContent();
        }

        //DELETE
        [HttpDelete("{pokeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeletePokemon(int pokeId)
        {

            var pokemonToDelete = _PokemonRepository.GetPokemon(pokeId);

            if (pokemonToDelete == null) return NotFound();

            if (!_PokemonRepository.DeletePokemon(pokemonToDelete))
                return StatusCode(500, "Something went wrong while deleting");

            return NoContent();
        }

    }
}
