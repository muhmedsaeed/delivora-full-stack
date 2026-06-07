namespace Delivora.DTOs;

public record OrderDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string DriverName { get; set; } = string.Empty;
    public string AddressTitle { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = new();
}


public record CreateOrderDto
{
    [Required]
    public int AddressId { get; set; }
    
    public int? DriverId { get; set; }
    
    [StringLength(500)]
    public string Notes { get; set; } = string.Empty;
    
    [Required, MinLength(1)]
    public List<CreateOrderItemDto> Items { get; set; } = new();
    
    [Required]
    public int PaymentMethodId { get; set; }
}



public record UpdateOrderStatusDto
{
    [Required]
    public OrderStatus Status { get; set; }
}




public record AssignDriverDto
{
    [Required]
    public int DriverId { get; set; }
}
