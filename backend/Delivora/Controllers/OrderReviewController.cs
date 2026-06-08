
namespace Delivora.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Customer")]
public class OrderReviewController : ControllerBase
{
    private readonly UnitOfWorks _unitOfWorks;
    private readonly IMapper _mapper;

    public OrderReviewController(UnitOfWorks uow, IMapper mapper)
    {
        _unitOfWorks = uow;
        _mapper = mapper;
    }


    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }


    [HttpGet("order/{orderId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByOrder(int orderId)
    {
        var reviews = await _unitOfWorks.OrderReviewRepository.GetAllAsync();

        var filtered = reviews.Where(r => r.OrderId == orderId);
        
        return Ok(_mapper.Map<List<OrderReviewDto>>(filtered));
    }



    [HttpPost]
    public async Task<IActionResult> Create(
    [FromBody] CreateOrderReviewDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var order = await _unitOfWorks.OrderRepository.GetByIdAsync(dto.OrderId);

        if (order is null || order.CustomerId != GetUserId())
            return BadRequest(new { message = "Invalid order." });

        if (order.Status != OrderStatus.Delivered)
            return BadRequest(new
            {
                message = "Order must be delivered."
            });
        
        var review = _mapper.Map<OrderReview>(dto);
        
        review.CustomerId = GetUserId();
        
        await _unitOfWorks.OrderReviewRepository.AddAsync(review);
        await _unitOfWorks.SaveChangesAsync();
        
        return Ok(_mapper.Map<OrderReviewDto>(review));
    }


}