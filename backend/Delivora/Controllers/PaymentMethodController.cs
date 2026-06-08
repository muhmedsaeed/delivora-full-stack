
namespace Delivora.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentMethodController : ControllerBase
{
    private readonly UnitOfWorks _unitOfWorks;
    private readonly IMapper _mapper;

    public PaymentMethodController(UnitOfWorks uow, IMapper mapper)
    {
        _unitOfWorks = uow;
        _mapper = mapper;
    }
    
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var methods = await _unitOfWorks.PaymentMethodRepository.GetAllAsync();

        return Ok(_mapper.Map<List<PaymentMethodDto>>(methods.Where(m => m.IsActive)));
    }



    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreatePaymentMethodDto dto)
    {
        var method = _mapper.Map<PaymentMethod>(dto);

        await _unitOfWorks.PaymentMethodRepository.AddAsync(method);
        await _unitOfWorks.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAll), new { id = method.Id }, _mapper.Map<PaymentMethodDto>(method));
    }

}