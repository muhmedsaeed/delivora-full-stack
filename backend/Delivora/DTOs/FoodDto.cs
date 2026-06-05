namespace Delivora.DTOs;

public record FoodDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    
    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }


    public Boolean IsAvailable { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

}


public record CreateFoodDto
{

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public IFormFile? Image { get; set; }


    public Boolean IsAvailable { get; set; }

    public int CategoryId { get; set; }

}



public record UpdateFoodDto
{

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public Boolean IsAvailable { get; set; }

    public int CategoryId { get; set; }

    public IFormFile? Image { get; set; }

}