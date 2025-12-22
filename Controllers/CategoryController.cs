using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }


        //GET
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<CategoryDto>> GetCategories()
        {

            var categoriesDb = _categoryRepository.GetCategories();

            if (categoriesDb == null || !categoriesDb.Any()) return NoContent();

            var categories = _mapper.Map<List<CategoryDto>>(categoriesDb);

            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CategoryDto> GetCategory(int categoryId)
        {

            var category = _categoryRepository.GetCategory(categoryId);

            if( category == null ) return NotFound();

            var mappedCategory = _mapper.Map<CategoryDto>(category);

            return Ok(mappedCategory);

        }

        [HttpGet("/pokeByCategory/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PokemonDto>> GetPokemonByCategoryId (int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId)) return NotFound();
            
            var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonsByCategory(categoryId));

            return Ok(pokemons);
        }

        //POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var normalizedName = categoryCreate.Name.Trim().ToUpper();

            var existingCategory = _categoryRepository.GetCategories()
                                                      .FirstOrDefault(c => c.Name.Trim().ToUpper() == normalizedName);

            if(existingCategory != null) return Conflict("Category already exists. ");

            var mappedCategory = _mapper.Map<Category>(categoryCreate);

            var created = _categoryRepository.CreateCategory(mappedCategory);

            if (!created) return StatusCode(500, "Something went wrong while saving. ");

            var responseDto = _mapper.Map<CategoryDto>(mappedCategory);

            return CreatedAtAction(
                      nameof(GetCategories),
                      new { id = mappedCategory.Id },
                      responseDto
             );
        }

        //PUT
        [HttpPut("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCategory(int categoryId, [FromBody]CategoryDto updatedCategory)
        {
            if (updatedCategory == null)
                return BadRequest("Category data is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(categoryId != updatedCategory.Id)
                return BadRequest("Route ID and body ID do not match");

            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();
            var mappedCategory = _mapper.Map<Category>(updatedCategory);

            if (!_categoryRepository.UpdateCategory(mappedCategory)) return StatusCode(500, "Something went wrong while saving");

            return NoContent();
        }

        //DELETE
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategory(int categoryId)
        {

            var categoryToDelete = _categoryRepository.GetCategory(categoryId);

            if (categoryToDelete == null) return NotFound(); 

            if (!_categoryRepository.DeleteCategory(categoryToDelete))
                return StatusCode(500, "Something went wrong while deleting");

            return NoContent();
        }

    }
}
