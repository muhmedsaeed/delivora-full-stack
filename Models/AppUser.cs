
namespace Delivora.Models;

public class AppUser : IdentityUser<int>
{
    public string FullName { get; set; } = string.Empty;

    public UserStatus Status { get; set; }

    public string? ProfileImageUrl { get; set; }

    public DateTime BirthDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;



    // Relationships
    public Customer? Customer { get; set; }

    public Admin? Admin { get; set; }

    public Driver? Driver { get; set; }




}
