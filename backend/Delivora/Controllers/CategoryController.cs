
namespace Delivora.Controllers;


[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class CategoryController : ControllerBase
{
    private readonly UnitOfWorks _unitOfWorks;
    private readonly IFileService _fileService;
    private readonly IMapper _map;

    public CategoryController(UnitOfWorks unitOfWorks, IFileService fileService, IMapper map)
    {
        _unitOfWorks = unitOfWorks;
        _fileService = fileService;
        _map = map;
    }


    // api/category
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _unitOfWorks.CategoryRepository.GetAllAsync();


        return Ok(_map.Map<List<CategoryDto>>(categories));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        Category? category = await _unitOfWorks.CategoryRepository.GetByIdAsync(id);
        if (category is null)
            return NotFound();

        return Ok(_map.Map<CategoryDto>(category));
    }


    [HttpGet("{name:alpha}")]
    public async Task<IActionResult> GetCategoryByName(string name)
    {
        Category? category = await _unitOfWorks.CategoryRepository.GetByNameAsync(name);
        if (category is null)
            return NotFound();

        return Ok(_map.Map<CategoryDto>(category));
    }



    // Add Category
    [HttpPost]
    public async Task<IActionResult> AddCategory([FromForm] CreateCategoryDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);


        var category = _map.Map<Category>(dto);
        
        category.ImageUrl = dto.Image is not null ? await _fileService.UploadFileAsync(dto.Image, "Images/Categories") : null;


        await _unitOfWorks.CategoryRepository.AddAsync(category);
        await _unitOfWorks._context.SaveChangesAsync();

        var categoryDto = _map.Map<CategoryDto>(category);

        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, categoryDto);
    }


    // Update Category
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromForm] UpdateCategoryDto dto)
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
