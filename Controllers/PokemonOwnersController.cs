using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonOwnersController : ControllerBase
    {
        private readonly IPokemonOwnersRepository pokemonOwnersRepository;
        private readonly IPokemonRepository pokemonRepository;
        private readonly IOwnerRepository ownerRepository;
        private readonly IMapper mapper;

        public PokemonOwnersController(IPokemonOwnersRepository pokemonOwnersRepository, IPokemonRepository pokemonRepository,
                         IOwnerRepository ownerRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.pokemonRepository = pokemonRepository;
            this.ownerRepository = ownerRepository;
            this.pokemonOwnersRepository = pokemonOwnersRepository;
        }

        //POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult AddPokemonOwner([FromBody] PokemonOwnersDto pokemonOwnerCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonExists = pokemonRepository.GetPokemon(pokemonOwnerCreate.PokemonId);
            var ownerExists = ownerRepository.GetOwner(pokemonOwnerCreate.OwnerId);

            if (pokemonExists == null || ownerExists == null) return NotFound(new { message = "Pokemon or Owner not found" });

            if (pokemonOwnersRepository.Exist(pokemonOwnerCreate.PokemonId, pokemonOwnerCreate.OwnerId))
            {
                return Conflict(new { message = "Relation already exists" });
            }

            var mappedPokemonOwner = mapper.Map<PokemonOwners>(pokemonOwnerCreate);

            if (!pokemonOwnersRepository.AddPokemonOwner(mappedPokemonOwner))
            {
                return StatusCode(500, new { message = "Failed to save" });
            }

            return Ok("pokemon and owner linked successfully.");
        }
    }
}