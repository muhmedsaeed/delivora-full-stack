using System.ComponentModel.DataAnnotations.Schema;

namespace Delivora.Models;

public class Food
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public Boolean IsAvailable { get; set; }



    // Relationships
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; } = default!;


    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();


    public List<FoodReview> FoodReviews { get; set; } = new List<FoodReview>();



}
