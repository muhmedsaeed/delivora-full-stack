namespace Delivora.DTOs;

public class CategoryDto
{
    [Required, StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }


    [StringLength(800)]
    public string Description { get; set; } = string.Empty;
}
