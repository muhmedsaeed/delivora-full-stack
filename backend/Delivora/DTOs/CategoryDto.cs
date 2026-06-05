namespace Delivora.DTOs;

public record CategoryDto
{
    public string Name { get; set; } = string.Empty;

    public string? ImageUrl { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<string> FoodList { get; set; } = new List<string>();

}


public record CreateCategoryDto
{
    [Required, StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    public IFormFile? Image { get; set; }

    [StringLength(800)]
    public string Description { get; set; } = string.Empty;
}


public record UpdateCategoryDto
{
    [Required, StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    public IFormFile? Image { get; set; }


    [StringLength(800)]
    public string Description { get; set; } = string.Empty;
}
