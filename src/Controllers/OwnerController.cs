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
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository ownerRepository;
        private readonly IMapper mapper;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper)
        {
            this.ownerRepository = ownerRepository;
            this.mapper = mapper;
        }

        //GET
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OwnerDto>>> GetOwners()
        {
            var ownersDb = await ownerRepository.GetOwnersAsync();

            if (!ownersDb.Any())
                return Ok(new List<OwnerDto>());

            var owners = mapper.Map<List<OwnerDto>>(ownersDb);

            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<OwnerDto>> GetOwner(int ownerId)
        {
            var ownerDb = await ownerRepository.GetOwnerAsync(ownerId);

            if (ownerDb == null)
                return NotFound();

            var owner = mapper.Map<OwnerDto>(ownerDb);

            return Ok(owner);
        }

        [HttpGet("OwnerByPokemon/{pokeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<OwnerDto>>> GetOwnersOfPokemon(int pokeId)
        {
            var ownersDb = await ownerRepository.GetOwnerOfPokemonAsync(pokeId);

            if (!ownersDb.Any())
                return Ok(new List<OwnerDto>());

            var owners = mapper.Map<List<OwnerDto>>(ownersDb);

            return Ok(owners);
        }

        [HttpGet("PokemonByOwner/{ownerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PokemonDto>>> GetPokemonByOwner(int ownerId)
        {
            var ownersDb = await ownerRepository.GetPokemonByOwnerAsync(ownerId);

            if (!ownersDb.Any())
                return Ok(new List<PokemonDto>());

            var pokemons = mapper.Map<List<PokemonDto>>(ownersDb);

            return Ok(pokemons);
        }


        //POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOwner([FromBody] OwnerCreateDto ownerCreate)
        {
            if (ownerCreate == null)
                return BadRequest("Owner data is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(ownerCreate.Name))
                return BadRequest("Owner name is required.");

            var existingOwner = await ownerRepository.OwnerExistByNameAsync(ownerCreate.Name.Trim().ToUpper());

            if (existingOwner)
                return Conflict(" Owner already exists. ");

            var mappedOwner = mapper.Map<Owner>(ownerCreate);

            var created = await ownerRepository.CreateOwnerAsync(mappedOwner);

            if (!created)
                return StatusCode(500, "Something went wrong while saving. ");

            var responseDto = mapper.Map<OwnerDto>(mappedOwner);

            return CreatedAtAction(
                          nameof(GetOwner),
                          new { ownerId = mappedOwner.Id },
                          responseDto);
        }


        //PUT
        [HttpPut("{ownerId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOwner(int ownerId, [FromBody] OwnerDto updatedOwner)
        {
            if (updatedOwner == null)
                return BadRequest("Owner data is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (ownerId != updatedOwner.Id)
                return BadRequest("Route ID and body ID do not match");

            if (!await ownerRepository.OwnerExistAsync(ownerId))
                return NotFound();
            var mappedOwner = mapper.Map<Owner>(updatedOwner);

            if (!await ownerRepository.UpdateOwnerAsync(mappedOwner))
                return StatusCode(500, "Something went wrong while saving");

            return NoContent();
        }

        //DELETE
        [HttpDelete("{ownerId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOwner(int ownerId)
        {

            var ownerToDelete = await ownerRepository.GetOwnerAsync(ownerId);

            if (ownerToDelete == null)
                return NotFound();

            if (!await ownerRepository.DeleteOwnerAsync(ownerToDelete))
                return StatusCode(500, "Something went wrong while deleting");

            return NoContent();
        }
    }
}