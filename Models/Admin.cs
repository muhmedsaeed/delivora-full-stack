using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivora.Models;

public class Admin
{
    [Key, ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public AppUser User { get; set; } = null!;
}
