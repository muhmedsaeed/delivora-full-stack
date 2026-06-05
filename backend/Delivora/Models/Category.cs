namespace Delivora.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? ImageUrl { get; set; } = string.Empty;

    // Relationships
    public List<Food> Foods { get; set; } = new List<Food>();




}
