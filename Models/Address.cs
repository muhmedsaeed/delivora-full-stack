using System.ComponentModel.DataAnnotations.Schema;

namespace Delivora.Models;


public class Address
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = "Egypt";


    // Relationships
    public int CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer Customer { get; set; } = null!;



    public List<Order> Orders { get; set; } = new List<Order>();



}

