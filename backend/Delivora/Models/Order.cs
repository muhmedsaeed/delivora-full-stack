

namespace Delivora.Models;

public class Order
{
    public int Id { get; set; }

    public OrderStatus Status { get; set; }

    public decimal SubTotal { get; set; }

    public decimal DeliveryFee { get; set; }

    public decimal Tax { get; set; }
    
    public decimal Total { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string Notes { get; set; } = string.Empty;




    // Relationships
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public Payment Payment { get; set; } = default!;


    public List<OrderReview> OrderReviews { get; set; } = new List<OrderReview>();



    public int AddressId { get; set; }

    [ForeignKey(nameof(AddressId))]
    public Address Address { get; set; } = null!;



    public int CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer Customer { get; set; } = null!;



    public int DriverId { get; set; }

    [ForeignKey(nameof(DriverId))]
    public Driver Driver { get; set; } = null!;




}
