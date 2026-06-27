namespace Delivora.Controllers;

[Route("api/admin/users")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminUsersController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly DeliveryContext _context;

    public AdminUsersController(UserManager<AppUser> userManager, DeliveryContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] string? role = null)
    {
        var users = await _context.Users
            .Include(u => u.Driver)
            .Where(u => u.Admin == null)
            .ToListAsync();

        var result = new List<AdminUserDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (!string.IsNullOrWhiteSpace(role) && !roles.Contains(role))
                continue;

            result.Add(new AdminUserDto
            {
                Id = user.Id,
                Username = user.UserName ?? string.Empty,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                Status = user.Status.ToString(),
                ProfileImageUrl = user.ProfileImageUrl,
                Roles = roles,
                VehicleType = user.Driver?.VehicleType.ToString(),
                LicenseNumber = user.Driver?.LicenseNumber
            });
        }

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _context.Users
            .Include(u => u.Driver)
            .FirstOrDefaultAsync(u => u.Id == id && u.Admin == null);
        if (user is null) return NotFound();

        return Ok(new AdminUserDto
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber,
            Status = user.Status.ToString(),
            ProfileImageUrl = user.ProfileImageUrl,
            Roles = await _userManager.GetRolesAsync(user),
            VehicleType = user.Driver?.VehicleType.ToString(),
            LicenseNumber = user.Driver?.LicenseNumber
        });
    }

    [HttpPut("{id:int}/lock")]
    public async Task<IActionResult> Lock(int id)
    {
        var user = await _context.Users
            .Include(u => u.Driver)
            .FirstOrDefaultAsync(u => u.Id == id && u.Admin == null);
        if (user is null) return NotFound();

        user.Status = UserStatus.Suspended;
        if (user.Driver is not null)
            user.Driver.IsAvailable = false;

        await _userManager.UpdateAsync(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id:int}/unlock")]
    public async Task<IActionResult> Unlock(int id)
    {
        var user = await _context.Users
            .Include(u => u.Driver)
            .FirstOrDefaultAsync(u => u.Id == id && u.Admin == null);
        if (user is null) return NotFound();

        user.Status = UserStatus.Active;
        if (user.Driver is not null)
            user.Driver.IsAvailable = true;

        await _userManager.UpdateAsync(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
