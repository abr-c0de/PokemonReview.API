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
        public ActionResult<IEnumerable<ReviewerDto>> GetReviewers()
        {

            var reviewersDb = reviewerRepository.GetReviewers();

            if (reviewersDb == null || !reviewersDb.Any()) return NoContent();

            var reviewers = mapper.Map<List<ReviewerDto>>(reviewersDb);

            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ReviewerDto> GetReviewer(int reviewerId)
        {

            var reviewerDb = reviewerRepository.GetReviewer(reviewerId);

            if (reviewerDb == null) return NotFound();

            var reviewer = mapper.Map<ReviewerDto>(reviewerDb);

            return Ok(reviewer);

        }

        [HttpGet("{reviewerId}/ReviewsByReviewer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<ReviewDto>> GetReviewsByReviewer(int reviewerId)
        {
            var reviewsDb = reviewerRepository.GetReviewsByReviewer(reviewerId);

            if (reviewsDb == null || !reviewsDb.Any()) return NotFound();

            var reviews = mapper.Map<List<ReviewDto>>(reviewsDb);

            return Ok(reviews);
        }

        //POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerCreate)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var normalizedName = reviewerCreate.FirstName.Trim().ToUpper();

            var existingReviewer = reviewerRepository.GetReviewers()
                                               .FirstOrDefault(r => r.FirstName.ToUpper() == normalizedName);

            if (existingReviewer != null) return Conflict(" Reviewer already exists. ");

            var mappedReviewer = mapper.Map<Reviewer>(reviewerCreate);

            var created = reviewerRepository.CreateReviewer(mappedReviewer);

            if (!created) return StatusCode(500, "Something went wrong while saving. ");

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
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto updatedReviewer)
        {
            if (updatedReviewer == null)
                return BadRequest("Reviewer data is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (reviewerId != updatedReviewer.Id)
                return BadRequest("Route ID and body ID do not match");

            if (!reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();
            var mappedReviewer = mapper.Map<Reviewer>(updatedReviewer);

            if (!reviewerRepository.UpdateReviewer(mappedReviewer)) return StatusCode(500, "Something went wrong while saving");

            return NoContent();
        }

        //DELETE
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteReviewer(int reviewerId)
        {

            var ReviewerToDelete = reviewerRepository.GetReviewer(reviewerId);

            if (ReviewerToDelete == null) return NotFound();

            if (!reviewerRepository.DeleteReviewer(ReviewerToDelete))
                return StatusCode(500, "Something went wrong while deleting");

            return NoContent();
        }
    }
}