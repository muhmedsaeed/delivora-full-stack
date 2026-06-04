
namespace Delivora.Models;

public class OrderReview
{
    public int Id { get; set; }

    public int PackagingRate { get; set; }

    public int DeliveryRate { get; set; }

    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;


    // Relationsips
    public int OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = null!;



    public int CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer Customer { get; set; } = null!;




}
