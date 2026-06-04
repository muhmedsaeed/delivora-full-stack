
using Delivora.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Delivora.Controllers;


[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class CategoryController : ControllerBase
{
    private readonly UnitOfWorks _unitOfWorks;
    public CategoryController(UnitOfWorks unitOfWorks)
    {
        _unitOfWorks = unitOfWorks;
    }


    // api/category
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _unitOfWorks.CategoryRepository.GetAllAsync();
        return Ok(categories);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var category = await _unitOfWorks.CategoryRepository.GetByIdAsync(id);
        if (category is null)
            return NotFound();

        return Ok(category);
    }


    // Add Category
    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] CategoryDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };

        await _unitOfWorks.CategoryRepository.AddAsync(category);
        await _unitOfWorks._context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
    }


    // Update Category
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);
        var existingCategory = await _unitOfWorks.CategoryRepository.GetByIdAsync(id);
        if (existingCategory is null)
            return NotFound();
        existingCategory.Name = dto.Name;
        existingCategory.Description = dto.Description;
        await _unitOfWorks.CategoryRepository.UpdateAsync(existingCategory);
        await _unitOfWorks._context.SaveChangesAsync();
        return NoContent();
    }


    // Delete Category
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var existingCategory = await _unitOfWorks.CategoryRepository.GetByIdAsync(id);
        if (existingCategory is null)
            return NotFound();
        await _unitOfWorks.CategoryRepository.DeleteAsync(existingCategory);
        await _unitOfWorks._context.SaveChangesAsync();
        return NoContent();
    }


}
