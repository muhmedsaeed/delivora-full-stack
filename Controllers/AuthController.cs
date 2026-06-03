

namespace Delivora.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly DeliveryContext _context;

    public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, DeliveryContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _context = context;
    }


    // api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var user = new AppUser
        {
            UserName = dto.Username,
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            BirthDate = dto.BirthDate,
            Status = UserStatus.Active,
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);
            return ValidationProblem(ModelState);
        }

        await _userManager.AddToRoleAsync(user, "Customer");

        _context.Customers.Add(new Customer { UserId = user.Id }); // Create associated Customer record

        if (!string.IsNullOrWhiteSpace(dto.Street))
        {
            _context.Addresses.Add(new Address
            {
                Title = dto.AddressTitle,
                Street = dto.Street,
                City = dto.City ?? string.Empty,
                Country = dto.Country,
                CustomerId = user.Id
            });
        }

        await _context.SaveChangesAsync();

        var (token, expiresAt) = await _tokenService.GenerateTokenAsync(user);

        return Ok(new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            Username = user.UserName!,
            Email = user.Email!,
            FullName = user.FullName,
            Roles = await _userManager.GetRolesAsync(user)
        });
    }



    // api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var user = await _userManager.FindByNameAsync(dto.Username);
        if (user is null)
            return Unauthorized(new { message = "Invalid username or password." });

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);

        if (result.IsLockedOut)
            return StatusCode(423, new { message = "Account locked. Try again later." });

        if (!result.Succeeded)
            return Unauthorized(new { message = "Invalid username or password." });

        var (token, expiresAt) = await _tokenService.GenerateTokenAsync(user);

        return Ok(new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            Username = user.UserName!,
            Email = user.Email!,
            FullName = user.FullName,
            Roles = await _userManager.GetRolesAsync(user)
        });
    }
}
