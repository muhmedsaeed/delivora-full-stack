namespace Delivora.DTOs;

public record OrderItemDto
{
    public int Id { get; set; }
    public int FoodId { get; set; }
    public string FoodName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}


public record CreateOrderItemDto
{
    [Required]
    public int FoodId { get; set; }
    
    [Range(1, 50)]
    public int Quantity { get; set; }
}
