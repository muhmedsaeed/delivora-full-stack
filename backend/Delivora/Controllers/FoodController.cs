

namespace Delivora.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FoodController : ControllerBase
{
    private readonly UnitOfWorks _unitOfWorks;
    private readonly IFileService _fileService;
    public FoodController(UnitOfWorks unitOfWorks, IFileService fileService)
    {
        _unitOfWorks = unitOfWorks;
        _fileService = fileService;
    }



    [HttpGet]
    public async Task<IActionResult> GetAllFoods()
    {
        var foods = await _unitOfWorks.FoodRepository.GetAllAsync();

        var foodDtos = foods.Select(f => new FoodDto
        {
            Id = f.Id,
            Name = f.Name,
            Description = f.Description,
            Price = f.Price,
            IsAvailable = f.IsAvailable,
            ImageUrl = f.ImageUrl,
            CategoryId = f.CategoryId,
            CategoryName = f.Category.Name
        }).ToList();
        return Ok(foodDtos);
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetFoodById(int id)
    {
        Food? food = await _unitOfWorks.FoodRepository.GetByIdAsync(id);
        if (food is null)
            return NotFound($"Food with ID '{id}' not found.");

        var foodDto = new FoodDto
        {
            Id = food.Id,
            Name = food.Name,
            Description = food.Description,
            Price = food.Price,
            IsAvailable = food.IsAvailable,
            ImageUrl = food.ImageUrl,
            CategoryId = food.CategoryId,
            CategoryName = food.Category.Name
        };

        return Ok(foodDto);
    }


    [HttpGet("{name:alpha}")]
    public async Task<IActionResult> GetFoodByName(string name)
    {
        Food? food = await _unitOfWorks.FoodRepository.GetByNameAsync(name);
        if (food is null)
            return NotFound($"Food with name '{name}' not found.");

        var foodDto = new FoodDto
        {
            Id = food.Id,
            Name = food.Name,
            Description = food.Description,
            Price = food.Price,
            IsAvailable = food.IsAvailable,
            ImageUrl = food.ImageUrl,
            CategoryId = food.CategoryId,
            CategoryName = food.Category.Name
        };

        return Ok(foodDto);
    }


    [HttpGet("bycategory/{categoryId:int}")]
    public async Task<IActionResult> GetFoodsByCategoryId(int categoryId)
    {
        var foods = await _unitOfWorks.FoodRepository.GetFoodsByCategoryIdAsync(categoryId);

        var foodDtos = foods.Select(f => new FoodDto
        {
            Id = f.Id,
            Name = f.Name,
            Description = f.Description,
            Price = f.Price,
            IsAvailable = f.IsAvailable,
            ImageUrl = f.ImageUrl,
            CategoryId = f.CategoryId,
            CategoryName = f.Category.Name
        }).ToList();

        return Ok(foodDtos);
    }


    [HttpGet("bycategory/{categoryName:alpha}")]
    public async Task<IActionResult> GetFoodsByCategoryName(string categoryName)
    {
        var category = await _unitOfWorks.CategoryRepository.GetByNameAsync(categoryName);
        if (category is null)
            return NotFound($"Category with name '{categoryName}' not found.");

        var foods = await _unitOfWorks.FoodRepository.GetFoodsByCategoryIdAsync(category.Id);
        var foodDtos = foods.Select(f => new FoodDto
        {
            Id = f.Id,
            Name = f.Name,
            Description = f.Description,
            Price = f.Price,
            IsAvailable = f.IsAvailable,
            ImageUrl = f.ImageUrl,
            CategoryId = f.CategoryId,
            CategoryName = f.Category.Name
        }).ToList();
        return Ok(foodDtos);
    }


    [HttpPost]
    public async Task<IActionResult> CreateFood([FromBody] CreateFoodDto foodDto)
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
        var food = new Food
        {
            Name = foodDto.Name,
            Description = foodDto.Description,
            Price = foodDto.Price,
            IsAvailable = foodDto.IsAvailable,
            ImageUrl = imageUrl,
            CategoryId = foodDto.CategoryId
        };

        await _unitOfWorks.FoodRepository.AddAsync(food);
        await _unitOfWorks.SaveChangesAsync();

        // Map the created Food entity to FoodDto for response        
        var createdFoodDto = new FoodDto
        {
            Id = food.Id,
            Name = food.Name,
            Description = food.Description,
            Price = food.Price,
            IsAvailable = food.IsAvailable,
            ImageUrl = food.ImageUrl,
            CategoryId = food.CategoryId,
            CategoryName = category.Name
        };

        return CreatedAtAction(nameof(GetFoodById), new { id = food.Id }, createdFoodDto);
    }



    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateFood(int id, [FromBody] UpdateFoodDto foodDto)
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
        if (category is not null)
        {
            food.CategoryId = foodDto.CategoryId;
        }

        // Handle file upload if Image is provided
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

        // Map the updated Food entity to FoodDto for response
        var updatedFoodDto = new FoodDto
        {
            Id = food.Id,
            Name = food.Name,
            Description = food.Description,
            Price = food.Price,
            IsAvailable = food.IsAvailable,
            ImageUrl = food.ImageUrl,
            CategoryId = food.CategoryId,
            CategoryName = food.Category.Name
        };

        return Ok(updatedFoodDto);
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteFood(int id)
    {
        var food = await _unitOfWorks.FoodRepository.GetByIdAsync(id);
        if (food is null)
            return NotFound($"Food with ID '{id}' not found.");
        
        // Delete the associated image if it exists
        if (!string.IsNullOrEmpty(food.ImageUrl))
        {
            await _fileService.DeleteFileAsync(food.ImageUrl);
        }
        await _unitOfWorks.FoodRepository.DeleteAsync(food);
        await _unitOfWorks.SaveChangesAsync();
        return NoContent();
    }
}
