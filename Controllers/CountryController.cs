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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<CountryDto>> GetCountries()
        {
            var countriesDb = countryRepository.GetCountries();

            if(countriesDb == null || !countriesDb.Any()) return NoContent();

            var countries = mapper.Map<List<CountryDto>>(countriesDb);

            return Ok(countries);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CountryDto> GetCountry(int id)
        {
            var countryDb = countryRepository.GetCountry(id);

            if(countryDb == null) return NotFound();

            var country = mapper.Map<CountryDto>(countryDb);

            return Ok(country);
        }

        [HttpGet("/ByOwner/{ownerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        public ActionResult<CountryDto> GetCountryByOwner(int ownerId)
        {
            var countryDb = countryRepository.GetCountryByOwner(ownerId);

            if(countryDb == null) return NotFound();

            var country = mapper.Map<CountryDto> (countryDb);

            return Ok(country);
        }

        [HttpGet("/OwnersByCountry/{countryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<OwnerDto>> GetOwnersByCountry(int countryId)
        {
            var ownersDb = countryRepository.GetOwnersFromCountry(countryId);

            if(ownersDb == null || !ownersDb.Any()) return NotFound();

            var owners = mapper.Map<List<OwnerDto>>(ownersDb);

            return Ok(owners);
        }

        //POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var normalizedName = countryCreate.Name.Trim().ToUpper();

            var existingCountry = countryRepository.GetCountries()
                                                   .FirstOrDefault(c => c.Name.ToUpper() == normalizedName);

            if (existingCountry != null) return Conflict("Country already exists. ");

            var mappedCountry = mapper.Map<Country>(countryCreate);

            var created = countryRepository.CreateCountry(mappedCountry);

            if (!created) return StatusCode(500, "something went wrong while saving. ");

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
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto updatedCountry)
        {
            if (updatedCountry == null)
                return BadRequest("Country data is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (countryId != updatedCountry.Id)
                return BadRequest("Route ID and body ID do not match");

            if (!countryRepository.CountryExist(countryId))
                return NotFound();
            var mappedCountry = mapper.Map<Country>(updatedCountry);

            if (!countryRepository.UpdateCountry(mappedCountry)) return StatusCode(500, "Something went wrong while saving");

            return NoContent();
        }


        //DELETE
        [HttpDelete("{countryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCountry(int countryId)
        {

            var countryToDelete = countryRepository.GetCountry(countryId);

            if (countryToDelete == null) return NotFound();

            if (!countryRepository.DeleteCountry(countryToDelete))
                return StatusCode(500, "Something went wrong while deleting");

            return NoContent();
        }

    }
}