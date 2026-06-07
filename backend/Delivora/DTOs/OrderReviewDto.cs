namespace Delivora.DTOs;

public record OrderReviewDto
{
    public int Id { get; set; }
    public int PackagingRate { get; set; }
    public int DeliveryRate { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int OrderId { get; set; }
}


public record CreateOrderReviewDto
{
    [Required]
    public int OrderId { get; set; }
    
    [Range(1, 5)]
    public int PackagingRate { get; set; }
    
    [Range(1, 5)]
    public int DeliveryRate { get; set; }
    
    [StringLength(1000)]
    public string Comment { get; set; } = string.Empty;
}
