

namespace Delivora.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Driver,Admin")]
public class DriverController : ControllerBase
{
    private readonly UnitOfWorks _unitOfWorks;
    private readonly DeliveryContext _context;
    private readonly IMapper _mapper;


    public DriverController(UnitOfWorks uow, DeliveryContext context, IMapper mapper)
    {
        _unitOfWorks = uow;
        _context = context;
        _mapper = mapper;
    }


    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var drivers = await _context.Drivers
            .Include(d => d.User)
            .ToListAsync();

        return Ok(_mapper.Map<List<DriverDto>>(drivers));
    }


    [HttpGet("available")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAvailable()
    {
        var drivers = await _context.Drivers
            .Include(d => d.User)
            .Where(d => d.IsAvailable && d.User.Status == UserStatus.Active)
            .ToListAsync();

        return Ok(_mapper.Map<List<DriverDto>>(drivers));
    }


    [HttpPut("{id:int}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Approve(int id)
    {
        var driver = await _context.Drivers
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.UserId == id);

        if (driver is null) return NotFound();

        driver.User.Status = UserStatus.Active;
        driver.IsAvailable = true;

        await _unitOfWorks.SaveChangesAsync();

        return NoContent();
    }


    [HttpPut("availability")]
    public async Task<IActionResult> SetAvailability([FromBody] bool isAvailable)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var driver = await _unitOfWorks.DriverRepository.GetByIdAsync(userId);

        if (driver is null) return NotFound();
        
        driver.IsAvailable = isAvailable;
        await _unitOfWorks.DriverRepository.UpdateAsync(driver);
        await _unitOfWorks.SaveChangesAsync();
        
        return NoContent();
    }
}
