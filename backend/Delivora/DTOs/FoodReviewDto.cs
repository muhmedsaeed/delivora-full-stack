namespace Delivora.DTOs;

public record FoodReviewDto
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime LastUpdate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string FoodName { get; set; } = string.Empty;
    public int FoodId { get; set; }
}


public record CreateFoodReviewDto
{
    [Required]
    public int FoodId { get; set; }
    
    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(1000)]
    public string Comment { get; set; } = string.Empty;
}



