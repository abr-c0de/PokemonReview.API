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
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {

            var categoriesDb = await _categoryRepository.GetCategoriesAsync();

            if (!categoriesDb.Any())
                    return Ok(new List<CategoryDto>());

            var categories = _mapper.Map<List<CategoryDto>>(categoriesDb);

            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDto>> GetCategory(int categoryId)
        {

            var category = await _categoryRepository.GetCategoryAsync(categoryId);

            if( category == null )
                return NotFound();

            var mappedCategory = _mapper.Map<CategoryDto>(category);

            return Ok(mappedCategory);

        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PokemonDto>>> GetPokemonByCategoryId (int categoryId)
        {
            if (!await _categoryRepository.CategoryExistsAsync(categoryId))
                return Ok(new List<PokemonDto>());
            
            var pokemons = _mapper.Map<List<PokemonDto>>(await _categoryRepository.GetPokemonsByCategoryAsync(categoryId));

            return Ok(pokemons);
        }

        //POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto categoryCreate)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (categoryCreate == null)
                return BadRequest("Category data is required");

            if (string.IsNullOrWhiteSpace(categoryCreate.Name))
                return BadRequest("Category name is required.");

            var existingCategory = await _categoryRepository.CategoryExistByNameAsync(categoryCreate.Name.Trim().ToUpper());

            if(existingCategory) 
                return Conflict("Category already exists. ");

            var mappedCategory = _mapper.Map<Category>(categoryCreate);

            var created = await _categoryRepository.CreateCategoryAsync(mappedCategory);

            if (!created)
                return StatusCode(500, "Something went wrong while saving. ");

            var responseDto = _mapper.Map<CategoryDto>(mappedCategory);

            return CreatedAtAction(
                      nameof(GetCategory),
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
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody]CategoryDto updatedCategory)
        {
            if (updatedCategory == null)
                return BadRequest("Category data is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(categoryId != updatedCategory.Id)
                return BadRequest("Route ID and body ID do not match");

            if (!await _categoryRepository.CategoryExistsAsync(categoryId))
                return NotFound();
            var mappedCategory = _mapper.Map<Category>(updatedCategory);

            if (!await _categoryRepository.UpdateCategoryAsync(mappedCategory)) return StatusCode(500, "Something went wrong while saving");

            return NoContent();
        }

        //DELETE
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {

            var categoryToDelete = await _categoryRepository.GetCategoryAsync(categoryId);

            if (categoryToDelete == null)
                return NotFound(); 

            if (!await _categoryRepository.DeleteCategoryAsync(categoryToDelete))
                return StatusCode(500, "Something went wrong while deleting");

            return NoContent();
        }

    }
}
