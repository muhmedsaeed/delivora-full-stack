using System.ComponentModel.DataAnnotations.Schema;

namespace Delivora.Models;

public class Payment
{
    public int Id { get; set; }

    public decimal Amount { get; set; } = 0;

    public PaymentStatus Status { get; set; }

    public DateTime PaymentDate { get; set; } = DateTime.Now;


    // Relationships
    public int OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = default!;



    public int PaymentMethodId { get; set; }

    [ForeignKey(nameof(PaymentMethodId))]
    public PaymentMethod PaymentMethod { get; set; } = default!;

}
