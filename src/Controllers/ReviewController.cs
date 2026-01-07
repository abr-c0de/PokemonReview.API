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
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository reviewRepository;
        private readonly IMapper mapper;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
        {
            this.reviewRepository = reviewRepository;
            this.mapper = mapper;
        }

        //GET

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
        {

            var reviewsDb = await reviewRepository.GetReviewsAsync();

            if (!reviewsDb.Any())
                return Ok(new List<ReviewDto>());

            var reviews = mapper.Map<List<ReviewDto>>(reviewsDb);

            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewDto>> GetReview(int reviewId)
        {

            var reviewDb = await reviewRepository.GetReviewAsync(reviewId);

            if (reviewDb == null)
                return NotFound();

            var review = mapper.Map<ReviewDto>(reviewDb);

            return Ok(review);

        }

        [HttpGet("{pokemonId}/ReviewsByPokemon")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsOfPokemon(int pokemonId)
        {
            var reviewsDb = await reviewRepository.GetReviewsOfPokemonAsync(pokemonId);

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
        public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDto reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest("Review data is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingReview = await reviewRepository.ReviewExistsByReviewerAsync(reviewCreate.PokemonId, reviewCreate.ReviewerId);

            if (existingReview)
                return Conflict(" Review already exists. ");

            var mappedReview = mapper.Map<Review>(reviewCreate);

            var created = await reviewRepository.CreateReviewAsync(mappedReview);

            if (!created)
                return StatusCode(500, "Something went wrong while saving. ");

            var responseDto = mapper.Map<ReviewDto>(mappedReview);

            return CreatedAtAction(
                          nameof(GetReview),
                          new { reviewId = mappedReview.Id },
                          responseDto);
        }


        //PUT
        [HttpPut("{reviewId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
                return BadRequest("Review data is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (reviewId != updatedReview.Id)
                return BadRequest("Route ID and body ID do not match");

            if (!await reviewRepository.ReviewExistsAsync(reviewId))
                return NotFound();

            var mappedReview = mapper.Map<Review>(updatedReview);

            if (!await reviewRepository.UpdateReviewAsync(mappedReview))
                return StatusCode(500, "Something went wrong while saving");

            return NoContent();
        }

        //DELETE
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {

            var ReviewToDelete = await reviewRepository.GetReviewAsync(reviewId);

            if (ReviewToDelete == null)
                return NotFound();

            if (!await reviewRepository.DeleteReviewAsync(ReviewToDelete))
                return StatusCode(500, "Something went wrong while deleting");

            return NoContent();
        }
    }
}