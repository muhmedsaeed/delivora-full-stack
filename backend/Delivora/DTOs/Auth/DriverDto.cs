namespace Delivora.DTOs.Auth;

public record DriverDto
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string VehicleType { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public decimal TotalEarnings { get; set; }
}



public record RegisterDriverDto
{
    [Required, StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;
    
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required, StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    public string FullName { get; set; } = string.Empty;
    
    [Phone]
    public string? PhoneNumber { get; set; }
    
    [Required]
    public VehicleType VehicleType { get; set; }
    
    [Required, StringLength(50)]
    public string LicenseNumber { get; set; } = string.Empty;
}
