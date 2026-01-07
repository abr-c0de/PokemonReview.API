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
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository countryRepository;
        private readonly IMapper mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            this.countryRepository = countryRepository;
            this.mapper = mapper;
        }


        //GET
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountries()
        {
            var countriesDb = await countryRepository.GetCountriesAsync();

            if (!countriesDb.Any())
                return Ok(new List<CountryDto>());

            var countries = mapper.Map<List<CountryDto>>(countriesDb);

            return Ok(countries);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var countryDb = await countryRepository.GetCountryAsync(id);

            if(countryDb == null)
                return NotFound();

            var country = mapper.Map<CountryDto>(countryDb);

            return Ok(country);
        }

        [HttpGet("ByOwner/{ownerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CountryDto>> GetCountryByOwner(int ownerId)
        {
            var countryDb = await countryRepository.GetCountryByOwnerAsync(ownerId);

            if(countryDb == null)
                return NotFound();

            var country = mapper.Map<CountryDto> (countryDb);

            return Ok(country);
        }

        [HttpGet("OwnersByCountry/{countryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OwnerDto>>> GetOwnersByCountry(int countryId)
        {
            var ownersDb = await countryRepository.GetOwnersFromCountryAsync(countryId);

            if(!ownersDb.Any())
                return Ok(new List<OwnerDto>());

            var owners = mapper.Map<List<OwnerDto>>(ownersDb);

            return Ok(owners);
        }


        //POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CountryCreateDto countryCreate)
        {

            if (countryCreate == null)
                return BadRequest("Country data is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(countryCreate.Name))
                return BadRequest("Country name is required.");

            var existingCountry = await countryRepository.CountryExistByNameAsync(countryCreate.Name.Trim().ToUpper());

            if (existingCountry)
                return Conflict("Country already exists. ");

            var mappedCountry = mapper.Map<Country>(countryCreate);

            var created = await countryRepository.CreateCountryAsync(mappedCountry);

            if (!created)
                return StatusCode(500, "something went wrong while saving. ");

            var responseDto = mapper.Map<CountryDto>(mappedCountry);

            return CreatedAtAction(
                      nameof(GetCountry),
                      new { id = mappedCountry.Id },
                      responseDto
             );

        }

        //PUT
        [HttpPut("{countryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int countryId, [FromBody] CountryDto updatedCountry)
        {
            if (updatedCountry == null)
                return BadRequest("Country data is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (countryId != updatedCountry.Id)
                return BadRequest("Route ID and body ID do not match");

            if (!await countryRepository.CountryExistAsync(countryId))
                return NotFound();
            var mappedCountry = mapper.Map<Country>(updatedCountry);

            if (!await countryRepository.UpdateCountryAsync(mappedCountry))
                return StatusCode(500, "Something went wrong while saving");

            return NoContent();
        }


        //DELETE
        [HttpDelete("{countryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int countryId)
        {

            var countryToDelete = await countryRepository.GetCountryAsync(countryId);

            if (countryToDelete == null)
                return NotFound();

            if (!await countryRepository.DeleteCountryAsync(countryToDelete))
                return StatusCode(500, "Something went wrong while deleting");

            return NoContent();
        }

    }
}