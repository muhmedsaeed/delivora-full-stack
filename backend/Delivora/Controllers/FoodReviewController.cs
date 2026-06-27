

namespace Delivora.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FoodReviewController : ControllerBase
{
    private readonly UnitOfWorks _unitOfWorks;
    private readonly IMapper _mapper;

    public FoodReviewController(UnitOfWorks uow, IMapper mapper)
    {
        _unitOfWorks = uow;
        _mapper = mapper;
    }


    [HttpGet("food/{foodId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByFood(int foodId)
    {
        var reviews = await _unitOfWorks.FoodReviewRepository.GetAllWithCustomerAsync();

        return Ok(_mapper.Map<List<FoodReviewDto>>(reviews.Where(r => r.FoodId == foodId)));
    }


    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Create([FromBody] CreateFoodReviewDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);
        
        var review = _mapper.Map<FoodReview>(dto);
        review.CustomerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        await _unitOfWorks.FoodReviewRepository.AddAsync(review);
        await _unitOfWorks.SaveChangesAsync();
        
        return Ok(_mapper.Map<FoodReviewDto>(review));
    }
}