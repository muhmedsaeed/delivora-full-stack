namespace Delivora.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly DeliveryContext _context;
    private readonly IFileService _fileService;

    public ProfileController(UserManager<AppUser> userManager, DeliveryContext context, IFileService fileService)
    {
        _userManager = userManager;
        _context = context;
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var user = await GetCurrentUserWithDetails();
        if (user is null) return Unauthorized();

        return Ok(await ToProfileDto(user));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdateProfileDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var user = await GetCurrentUserWithDetails();
        if (user is null) return Unauthorized();

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.UserName ??= dto.Email;
        user.PhoneNumber = dto.PhoneNumber;
        if (dto.BirthDate.HasValue)
            user.BirthDate = dto.BirthDate.Value;

        if (user.Driver is not null)
        {
            if (dto.VehicleType.HasValue)
                user.Driver.VehicleType = dto.VehicleType.Value;
            if (!string.IsNullOrWhiteSpace(dto.LicenseNumber))
                user.Driver.LicenseNumber = dto.LicenseNumber;
        }

        if (dto.RemoveImage && dto.ProfileImage is null && !string.IsNullOrEmpty(user.ProfileImageUrl))
        {
            await _fileService.DeleteFileAsync(user.ProfileImageUrl);
            user.ProfileImageUrl = null;
        }

        if (dto.ProfileImage is not null)
        {
            if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                await _fileService.DeleteFileAsync(user.ProfileImageUrl);
            user.ProfileImageUrl = await _fileService.UploadFileAsync(dto.ProfileImage, "Images/Profiles");
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);
            return ValidationProblem(ModelState);
        }

        await _context.SaveChangesAsync();

        return Ok(await ToProfileDto(user));
    }

    [HttpDelete("image")]
    public async Task<IActionResult> DeleteImage()
    {
        var user = await GetCurrentUserWithDetails();
        if (user is null) return Unauthorized();

        if (!string.IsNullOrEmpty(user.ProfileImageUrl))
        {
            await _fileService.DeleteFileAsync(user.ProfileImageUrl);
            user.ProfileImageUrl = null;
            await _userManager.UpdateAsync(user);
        }

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Deactivate()
    {
        var user = await GetCurrentUserWithDetails();
        if (user is null) return Unauthorized();

        user.Status = UserStatus.Inactive;
        if (user.Driver is not null)
            user.Driver.IsAvailable = false;

        await _userManager.UpdateAsync(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    private async Task<AppUser?> GetCurrentUserWithDetails()
    {
        var id = GetUserId();
        return await _context.Users
            .Include(u => u.Driver)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    private async Task<ProfileDto> ToProfileDto(AppUser user)
    {
        return new ProfileDto
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber,
            BirthDate = user.BirthDate,
            Status = user.Status.ToString(),
            ProfileImageUrl = user.ProfileImageUrl,
            Roles = await _userManager.GetRolesAsync(user),
            VehicleType = user.Driver?.VehicleType.ToString(),
            LicenseNumber = user.Driver?.LicenseNumber
        };
    }
}
