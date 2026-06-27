
namespace Delivora.DTOs.Auth;

public class RegisterDto
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100, MinimumLength = 8)]
    //[DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    //[DataType(DataType.Password)]
    public string PasswordConfirmed { get; set; } = string.Empty;

    [Required]
    [StringLength(20, MinimumLength = 10)]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required]
    public DateTime BirthDate { get; set; }

    public IFormFile? ProfileImage { get; set; }


    // Default address (optional at registration)
    [StringLength(50)]
    public string AddressTitle { get; set; } = "Home";

    [StringLength(150)]
    public string? Street { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(50)]
    public string Country { get; set; } = "Egypt";


}
