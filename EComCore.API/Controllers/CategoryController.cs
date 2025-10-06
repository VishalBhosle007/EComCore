using EComCore.Application.DTOs;
using EComCore.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EComCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await categoryService.GetByIdAsync(id);
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] InsertCategoryRequestDto categoryDto)
        {
            var category = await categoryService.CreateCategoryAsync(categoryDto);

            // for now ok response is sendng after implementing i will return createdAtAction response
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequestDto updatecategoryRequest)
        {
            if (id == Guid.Empty)
                return BadRequest(new { Message = "Category ID cannot be empty." });

            var result = await categoryService.UpdateCategoryAsync(id, updatecategoryRequest);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { Message = "Category ID cannot be empty." });

            var result = await categoryService.DeleteCategoryAsync(id);
            return Ok(result);
        }

    }
}