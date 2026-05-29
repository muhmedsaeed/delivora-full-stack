using System.ComponentModel.DataAnnotations.Schema;

namespace Delivora.Models;

public class OrderItem
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; } = decimal.Zero;

    public decimal TotalPrice { get; set; }




    // Relationships
    public int OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = default!;


    public int FoodId { get; set; }

    [ForeignKey(nameof(FoodId))]
    public Food Food { get; set; } = default!;





}
