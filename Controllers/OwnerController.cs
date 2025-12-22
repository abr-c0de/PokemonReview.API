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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<OwnerDto>> GetOwners()
        {
            var ownersDb = ownerRepository.GetOwners();

            if (ownersDb == null || !ownersDb.Any()) return NoContent();

            var owners = mapper.Map<List<OwnerDto>>(ownersDb);

            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<OwnerDto> GetOwner(int ownerId)
        {
            var ownerDb = ownerRepository.GetOwner(ownerId);

            if (ownerDb == null) return NotFound();

            var owner = mapper.Map<OwnerDto>(ownerDb);

            return Ok(owner);
        }

        [HttpGet("/OwnerByPokemon/{pokeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<OwnerDto>> GetOwnersOfPokemon(int pokeId)
        {
            var ownersDb = ownerRepository.GetOwnerOfPokemon(pokeId);

            if (ownersDb == null || !ownersDb.Any()) return NoContent();

            var owners = mapper.Map<List<OwnerDto>>(ownersDb);

            return Ok(owners);
        }

        [HttpGet("/PokemonByOwner/{ownerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<PokemonDto>> GetPokemonByOwner(int ownerId)
        {
            var ownersDb = ownerRepository.GetPokemonByOwner(ownerId);

            if (ownersDb == null || !ownersDb.Any()) return NoContent();

            var owners = mapper.Map<List<PokemonDto>>(ownersDb);

            return Ok(owners);
        }

        //POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateOwner([FromBody] OwnerCreateDto ownerCreate)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var normalizedName = ownerCreate.Name.Trim().ToUpper();

            var existingOwner = ownerRepository.GetOwners()
                                               .FirstOrDefault(o => o.Name.ToUpper() == normalizedName);

            if (existingOwner != null) return Conflict(" Owner already exists. ");

            var mappedOwner = mapper.Map<Owner>(ownerCreate);

            var created = ownerRepository.CreateOwner(mappedOwner);

            if (!created) return StatusCode(500, "Something went wrong while saving. ");

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
        public IActionResult UpdateCountry(int ownerId, [FromBody] OwnerUpdateDto updatedOwner)
        {
            if (updatedOwner == null)
                return BadRequest("Owner data is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (ownerId != updatedOwner.Id)
                return BadRequest("Route ID and body ID do not match");

            if (!ownerRepository.OwnerExist(ownerId))
                return NotFound();
            var mappedOwner = mapper.Map<Owner>(updatedOwner);

            if (!ownerRepository.UpdateOwner(mappedOwner)) return StatusCode(500, "Something went wrong while saving");

            return NoContent();
        }

        //DELETE
        [HttpDelete("{ownerId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteOwner(int ownerId)
        {

            var ownerToDelete = ownerRepository.GetOwner(ownerId);

            if (ownerToDelete == null) return NotFound();

            if (!ownerRepository.DeleteOwner(ownerToDelete))
                return StatusCode(500, "Something went wrong while deleting");

            return NoContent();
        }
    }
}