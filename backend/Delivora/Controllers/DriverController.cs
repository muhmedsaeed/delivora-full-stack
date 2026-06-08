

namespace Delivora.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Driver,Admin")]
public class DriverController : ControllerBase
{
    private readonly UnitOfWorks _unitOfWorks;
    private readonly IMapper _mapper;


    public DriverController(UnitOfWorks uow, IMapper mapper)
    {
        _unitOfWorks = uow;
        _mapper = mapper;
    }


    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var drivers = await _unitOfWorks.DriverRepository.GetAllAsync();

        return Ok(_mapper.Map<List<DriverDto>>(drivers));
    }


    [HttpGet("available")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAvailable()
    {
        var drivers = await _unitOfWorks.DriverRepository.GetAllAsync();

        return Ok(_mapper.Map<List<DriverDto>>(drivers.Where(d => d.IsAvailable)));
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