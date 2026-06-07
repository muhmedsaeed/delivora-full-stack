namespace Delivora.DTOs;

public record PaymentDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public int OrderId { get; set; }
    public string MethodName { get; set; } = string.Empty;
}


public record PaymentMethodDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}


public record UpdatePaymentStatusDto
{
    [Required]
    public PaymentStatus Status { get; set; }
}
