
namespace Delivora.Models;

public class Customer
{
    [Key, ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public AppUser User { get; set; } = null!;


    // Relationships
    public List<Address> Addresses { get; set; } = new List<Address>();


    public List<OrderReview> OrderReviews { get; set; } = new List<OrderReview>();


    public List<FoodReview> FoodReviews { get; set; } = new List<FoodReview>();


    public List<Order> Orders { get; set; } = new List<Order>();




}
