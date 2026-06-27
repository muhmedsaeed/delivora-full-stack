

namespace Delivora.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FoodController : ControllerBase
{
    private readonly UnitOfWorks _unitOfWorks;
    private readonly IFileService _fileService;
    private readonly IMapper _map;
    public FoodController(UnitOfWorks unitOfWorks, IFileService fileService, IMapper map)
    {
        _unitOfWorks = unitOfWorks;
        _fileService = fileService;
        _map = map;
    }



    [HttpGet]
    public async Task<IActionResult> GetAllFoods()
    {
        var foods = await _unitOfWorks.FoodRepository.GetAllWithCategoryAsync();
        
        return Ok(_map.Map<List<FoodDto>>(foods));
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetFoodById(int id)
    {
        Food? food = await _unitOfWorks.FoodRepository.GetByIdWithCategoryAsync(id);
        if (food is null)
            return NotFound($"Food with ID '{id}' not found.");

        return Ok(_map.Map<FoodDto>(food));
    }


    [HttpGet("{name:alpha}")]
    public async Task<IActionResult> GetFoodByName(string name)
    {
        Food? food = await _unitOfWorks.FoodRepository.GetByNameWithCategoryAsync(name);
        if (food is null)
            return NotFound($"Food with name '{name}' not found.");

        return Ok(_map.Map<FoodDto>(food));

    }


    [HttpGet("bycategory/{categoryId:int}")]
    public async Task<IActionResult> GetFoodsByCategoryId(int categoryId)
    {
        var foods = await _unitOfWorks.FoodRepository.GetFoodsByCategoryIdAsync(categoryId);

        return Ok(_map.Map<List<FoodDto>>(foods));
    }


    [HttpGet("bycategory/{categoryName}")]
    public async Task<IActionResult> GetFoodsByCategoryName(string categoryName)
    {
        var category = await _unitOfWorks.CategoryRepository.GetByNameAsync(categoryName);
        if (category is null)
            return NotFound($"Category with name '{categoryName}' not found.");

        var foods = await _unitOfWorks.FoodRepository.GetFoodsByCategoryIdAsync(category.Id);
        
        return Ok(_map.Map<List<FoodDto>>(foods));
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateFood([FromForm] CreateFoodDto foodDto)
    {
        // Implementation for creating a new food item
        var category = await _unitOfWorks.CategoryRepository.GetByIdAsync(foodDto.CategoryId);
        if (category is null)
            return NotFound($"Category with ID '{foodDto.CategoryId}' not found.");

        // Handle file upload if Image is provided
        string? imageUrl = null;
        if (foodDto.Image is not null)
        {
            imageUrl = await _fileService.UploadFileAsync(foodDto.Image, "Images/Foods");
        }

        // Create new Food entity
        var food = _map.Map<Food>(foodDto);
        food.CategoryId = category.Id;
        food.ImageUrl = imageUrl;

        await _unitOfWorks.FoodRepository.AddAsync(food);
        await _unitOfWorks.SaveChangesAsync();

        var created = await _unitOfWorks.FoodRepository.GetByIdWithCategoryAsync(food.Id);

        return CreatedAtAction(nameof(GetFoodById),
                                new { id = food.Id }, _map.Map<FoodDto>(created));
    }



    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateFood(int id, [FromForm] UpdateFoodDto foodDto)
    {
        var food = await _unitOfWorks.FoodRepository.GetByIdAsync(id);
        if (food is null)
            return NotFound($"Food with ID '{id}' not found.");

        // Update the food entity with the new values
        food.Name = foodDto.Name;
        food.Description = foodDto.Description;
        food.Price = foodDto.Price;
        food.IsAvailable = foodDto.IsAvailable;

        var category = await _unitOfWorks.CategoryRepository.GetByIdAsync(foodDto.CategoryId);
        if (category is null)
            return NotFound($"Category with ID '{foodDto.CategoryId}' not found.");

        food.CategoryId = foodDto.CategoryId;

        // Handle file upload if Image is provided
        if (foodDto.RemoveImage && foodDto.Image is null && !string.IsNullOrEmpty(food.ImageUrl))
        {
            await _fileService.DeleteFileAsync(food.ImageUrl);
            food.ImageUrl = null;
        }

        if (foodDto.Image is not null)
        {
            // Delete the old image if it exists
            if (!string.IsNullOrEmpty(food.ImageUrl))
            {
                await _fileService.DeleteFileAsync(food.ImageUrl);
            }

            // Upload the new image
            food.ImageUrl = await _fileService.UploadFileAsync(foodDto.Image, "Images/Foods");
        }

        await _unitOfWorks.SaveChangesAsync();

        var updated = await _unitOfWorks.FoodRepository.GetByIdWithCategoryAsync(food.Id);

        return Ok(_map.Map<FoodDto>(updated));
    }


    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteFood(int id)
    {
        var food = await _unitOfWorks.FoodRepository.GetByIdAsync(id);
        
        if (food is null)
            return NotFound($"Food with ID '{id}' not found.");

        
        if (!string.IsNullOrEmpty(food.ImageUrl))
            await _fileService.DeleteFileAsync(food.ImageUrl);

        await _unitOfWorks.FoodRepository.DeleteAsync(food);
        await _unitOfWorks.SaveChangesAsync();
        return NoContent();
    }
}
