namespace Delivora.DTOs;

public record AddressDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = "Egypt";
}


public record CreateAddressDto
{
    [Required, StringLength(50)]
    public string Title { get; set; } = "Home";
    
    [Required, StringLength(150)]
    public string Street { get; set; } = string.Empty;
    

    [Required, StringLength(100)]
    public string City { get; set; } = string.Empty;

    [StringLength(50)]
    public string Country { get; set; } = "Egypt";
}


public record UpdateAddressDto : CreateAddressDto
{
}