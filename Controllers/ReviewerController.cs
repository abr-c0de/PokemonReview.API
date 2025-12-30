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
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository reviewerRepository;
        private readonly IMapper mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            this.reviewerRepository = reviewerRepository;
            this.mapper = mapper;
        }


        //GET

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<ReviewerDto>>> GetReviewers()
        {

            var reviewersDb = await reviewerRepository.GetReviewersAsync();

            if (!reviewersDb.Any())
                return Ok(new List<ReviewerDto>());

            var reviewers = mapper.Map<List<ReviewerDto>>(reviewersDb);

            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewerDto>> GetReviewer(int reviewerId)
        {

            var reviewerDb = await reviewerRepository.GetReviewerAsync(reviewerId);

            if (reviewerDb == null)
                return NotFound();

            var reviewer = mapper.Map<ReviewerDto>(reviewerDb);

            return Ok(reviewer);

        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsByReviewer(int reviewerId)
        {
            var reviewsDb = await reviewerRepository.GetReviewsByReviewerAsync(reviewerId);

            if (!reviewsDb.Any())
                return Ok(new List<ReviewDto>());

            var reviews = mapper.Map<List<ReviewDto>>(reviewsDb);

            return Ok(reviews);
        }

        //POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReviewer([FromBody] ReviewerCreateDto reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest("Reviewer data is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingReviewer = await reviewerRepository.ReviewerExistByNameAsync(reviewerCreate.LastName.Trim().ToUpper());

            if (existingReviewer)
                return Conflict(" Reviewer already exists. ");

            var mappedReviewer = mapper.Map<Reviewer>(reviewerCreate);

            var created = await reviewerRepository.CreateReviewerAsync(mappedReviewer);

            if (!created)
                return StatusCode(500, "Something went wrong while saving. ");

            var responseDto = mapper.Map<ReviewerDto>(mappedReviewer);

            return CreatedAtAction(
                          nameof(GetReviewer),
                          new { reviewerId = mappedReviewer.Id },
                          responseDto);
        }

        //PUT
        [HttpPut("{reviewerId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReviewer(int reviewerId, [FromBody] ReviewerDto updatedReviewer)
        {
            if (updatedReviewer == null)
                return BadRequest("Reviewer data is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (reviewerId != updatedReviewer.Id)
                return BadRequest("Route ID and body ID do not match");

            if (!await reviewerRepository.ReviewerExistsAsync(reviewerId))
                return NotFound();

            var mappedReviewer = mapper.Map<Reviewer>(updatedReviewer);

            if (!await reviewerRepository.UpdateReviewerAsync(mappedReviewer))
                return StatusCode(500, "Something went wrong while saving");

            return NoContent();
        }

        //DELETE
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteReviewer(int reviewerId)
        {

            var reviewerToDelete = await reviewerRepository.GetReviewerAsync(reviewerId);

            if (reviewerToDelete == null)
                return NotFound();

            if (!await reviewerRepository.DeleteReviewerAsync(reviewerToDelete))
                return StatusCode(500, "Something went wrong while deleting");

            return NoContent();
        }
    }
}