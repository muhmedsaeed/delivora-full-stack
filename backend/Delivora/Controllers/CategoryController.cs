
namespace Delivora.Controllers;


[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class CategoryController : ControllerBase
{
    private readonly UnitOfWorks _unitOfWorks;
    private readonly IFileService _fileService;

    public CategoryController(UnitOfWorks unitOfWorks, IFileService fileService)
    {
        _unitOfWorks = unitOfWorks;
        _fileService = fileService;
    }


    // api/category
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _unitOfWorks.CategoryRepository.GetAllAsync();
        
        var categoryDtos = categories.Select(c => new CategoryDto
        {
            Name = c.Name,
            Description = c.Description,
            ImageUrl = c.ImageUrl,
            FoodList = c.Foods.Select(f => f.Name).ToList()
        }).ToList();

        return Ok(categoryDtos);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        Category? category = await _unitOfWorks.CategoryRepository.GetByIdAsync(id);
        if (category is null)
            return NotFound();

        var categoryDto = new CategoryDto
        {
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            FoodList = category.Foods.Select(f => f.Name).ToList()
        };

        return Ok(categoryDto);
    }


    [HttpGet("{name:alpha}")]
    public async Task<IActionResult> GetCategoryByName(string name)
    {
        Category? category = await _unitOfWorks.CategoryRepository.GetByNameAsync(name);
        if (category is null)
            return NotFound();

        var categoryDto = new CategoryDto
        {
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            FoodList = category.Foods.Select(f => f.Name).ToList()
        };

        return Ok(categoryDto);
    }



    // Add Category
    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] CreateCategoryDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            ImageUrl = dto.Image is not null ? await _fileService.UploadFileAsync(dto.Image, "Images/Categories") : null
        };

        await _unitOfWorks.CategoryRepository.AddAsync(category);
        await _unitOfWorks._context.SaveChangesAsync();

        var categoryDto = new CategoryDto
        {
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            FoodList = new List<string>()
        };

        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, categoryDto);
    }


    // Update Category
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var existingCategory = await _unitOfWorks.CategoryRepository.GetByIdAsync(id);
        if (existingCategory is null)
            return NotFound();

        existingCategory.Name = dto.Name;
        existingCategory.Description = dto.Description;
        
        if(dto.Image is not null)
        {
            if (!string.IsNullOrEmpty(existingCategory.ImageUrl)) // Delete the old image if it exists
                await _fileService.DeleteFileAsync(existingCategory.ImageUrl);
            existingCategory.ImageUrl = await _fileService.UploadFileAsync(dto.Image, "Images/Categories");
        }

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

        // Delete the associated image if it exists
        if (!string.IsNullOrEmpty(existingCategory.ImageUrl))
        {
            await _fileService.DeleteFileAsync(existingCategory.ImageUrl);
        }

        await _unitOfWorks.CategoryRepository.DeleteAsync(existingCategory);
        await _unitOfWorks._context.SaveChangesAsync();
        return NoContent();
    }


}
