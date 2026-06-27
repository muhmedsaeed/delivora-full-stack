namespace Delivora.DTOs;

public record ProfileDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
    public string? VehicleType { get; set; }
    public string? LicenseNumber { get; set; }
}

public record UpdateProfileDto
{
    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string? PhoneNumber { get; set; }

    public DateTime? BirthDate { get; set; }

    public VehicleType? VehicleType { get; set; }

    [StringLength(50)]
    public string? LicenseNumber { get; set; }

    public IFormFile? ProfileImage { get; set; }

    public bool RemoveImage { get; set; }
}

public record AdminUserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
    public string? VehicleType { get; set; }
    public string? LicenseNumber { get; set; }
}
