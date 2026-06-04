
namespace Delivora.Models;

public class Driver
{
    [Key, ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public AppUser User { get; set; } = null!;


    public VehicleType VehicleType { get; set; }

    public string LicenseNumber { get; set; } = string.Empty;

    public bool IsAvailable { get; set; } = true;

    public decimal TotalEarnings { get; set; } = 0;



    // Relationships
    public List<Order> Orders { get; set; } = new List<Order>();



}
