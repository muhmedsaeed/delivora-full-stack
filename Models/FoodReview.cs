using System.ComponentModel.DataAnnotations.Schema;

namespace Delivora.Models;

public class FoodReview
{
    public int Id { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    public DateTime LastUpdate { get; set; } = DateTime.Now;


    // Relationsips
    public int FoodId { get; set; }

    [ForeignKey(nameof(FoodId))]
    public Food Food { get; set; } = null!;



    public int CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer Customer { get; set; } = null!;



}
