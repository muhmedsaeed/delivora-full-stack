
namespace Delivora.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly UnitOfWorks _unitOfWorks;
    private readonly IMapper _mapper;


    public PaymentController(UnitOfWorks uow, IMapper mapper)
    {
        _unitOfWorks = uow;
        _mapper = mapper;
    }



    [HttpGet("order/{orderId:int}")]
    public async Task<IActionResult> GetByOrderId(int orderId)
    {
        var payments = await _unitOfWorks.PaymentRepository.GetAllAsync();

        var payment = payments.FirstOrDefault(p => p.OrderId == orderId);

        if (payment is null) 
            return NotFound();

        return Ok(_mapper.Map<PaymentDto>(payment));
    }



    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdatePaymentStatusDto dto)
    {
        var payment = await _unitOfWorks.PaymentRepository.GetByIdAsync(id);

        if (payment is null) return NotFound();
        
        payment.Status = dto.Status;
        payment.PaymentDate = DateTime.UtcNow;
        
        await _unitOfWorks.PaymentRepository.UpdateAsync(payment);
        await _unitOfWorks.SaveChangesAsync();
        
        return NoContent();
    }


}
