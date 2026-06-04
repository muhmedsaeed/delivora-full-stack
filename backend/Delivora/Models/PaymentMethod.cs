namespace Delivora.Models;

public class PaymentMethod
{
    public int Id { get; set; }

    public PaymentMethodsName Name { get; set; }

    public string Description { get; set; } = string.Empty;

    public Boolean IsActive { get; set; }


    // Relationships
    public List<Payment> Payments { get; set; } = null!;

}
