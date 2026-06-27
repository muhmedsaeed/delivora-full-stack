
namespace Delivora.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly UnitOfWorks _unitOfWorks;
    private readonly IMapper _map;
    
    private const decimal DeliveryFee = 25m;
    private const decimal TaxRate = 0.14m;
    
    
    public OrderController(UnitOfWorks uow, IMapper mapper)
    {
        _unitOfWorks = uow;
        _map = mapper;
    }



    // Helper methods
    private int GetUserId() {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    private bool IsAdmin() { 
        return User.IsInRole("Admin");
    }

    private bool IsDriver() {
        return User.IsInRole("Driver"); 
    }



    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        IEnumerable<Order> orders;

        if (IsAdmin())
        {
            orders = await _unitOfWorks.OrderRepository.GetAllWithDetailsAsync();
        }
        else if (IsDriver())
        {
            orders = await _unitOfWorks.OrderRepository.GetByDriverIdAsync(GetUserId());
        }
        else
        {
            orders = await _unitOfWorks.OrderRepository.GetByCustomerIdAsync(GetUserId());
        }

        return Ok(_map.Map<List<OrderDto>>(orders));
    }




    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _unitOfWorks.OrderRepository.GetByIdWithDetailsAsync(id);
        
        if (order is null) return NotFound();
        
        if (!IsAdmin() && order.CustomerId != GetUserId() && order.DriverId != GetUserId())
            return Forbid();


        return Ok(_map.Map<OrderDto>(order));
    }



    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var address = await _unitOfWorks.AddressRepository.GetByIdAsync(dto.AddressId);

        if (address is null || address.CustomerId != GetUserId())
            return BadRequest(new { message = "Invalid address." });

        var method = await _unitOfWorks.PaymentMethodRepository.GetByIdAsync(dto.PaymentMethodId);


        if (method is null || !method.IsActive)
            return BadRequest(new { message = "Invalid payment." });


        decimal subTotal = 0;
        var orderItems = new List<OrderItem>();

        foreach (var item in dto.Items)
        {
            var food = await _unitOfWorks.FoodRepository.GetByIdAsync(item.FoodId);
            
            if (food is null || !food.IsAvailable)
                return BadRequest(new
                {
                    message = $"Food {item.FoodId} unavailable."
                });

            var lineTotal = food.Price * item.Quantity;
            subTotal += lineTotal;

            orderItems.Add(new OrderItem
            {
                FoodId = food.Id,
                Quantity = item.Quantity,
                UnitPrice = food.Price,
                TotalPrice = lineTotal
            });
        }

        var tax = Math.Round(subTotal * TaxRate, 2);
        var total = subTotal + DeliveryFee + tax;
        
        var order = new Order
        {
            Status = OrderStatus.Pending,
            SubTotal = subTotal,
            DeliveryFee = DeliveryFee,
            Tax = tax,
            Total = total,
            Notes = dto.Notes,
            AddressId = dto.AddressId,
            CustomerId = GetUserId(),
            DriverId = dto.DriverId,
            OrderItems = orderItems,
            Payment = new Payment
            {
                Amount = total,
                Status = PaymentStatus.Pending,
                PaymentMethodId = dto.PaymentMethodId
            }
        };

        await _unitOfWorks.OrderRepository.AddAsync(order);
        await _unitOfWorks.SaveChangesAsync();

        var created = await _unitOfWorks.OrderRepository.GetByIdWithDetailsAsync(order.Id);

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, _map.Map<OrderDto>(created));
    }



    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Admin,Driver")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDto dto)
    {
        var order = await _unitOfWorks.OrderRepository.GetByIdAsync(id);

        if (order is null) return NotFound();
        
        order.Status = dto.Status;
        
        await _unitOfWorks.SaveChangesAsync();

        var updated = await _unitOfWorks.OrderRepository.GetByIdWithDetailsAsync(order.Id);
        return Ok(_map.Map<OrderDto>(updated));
    }




    [HttpPut("{id:int}/assign-driver")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignDriver(int id, [FromBody] AssignDriverDto dto)
    {
        var order = await _unitOfWorks.OrderRepository.GetByIdAsync(id);

        if (order is null) return NotFound();
        
        var driver = await _unitOfWorks.DriverRepository.GetByIdAsync(dto.DriverId);

        if (driver is null || !driver.IsAvailable)
            return BadRequest(new { message = "Driver unavailable." });

        order.DriverId = dto.DriverId;
        order.Status = OrderStatus.Confirmed;

        await _unitOfWorks.SaveChangesAsync();

        var updated = await _unitOfWorks.OrderRepository.GetByIdWithDetailsAsync(order.Id);
        return Ok(_map.Map<OrderDto>(updated));
    }




    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Customer,Admin")]
    public async Task<IActionResult> Cancel(int id)
    {
        var order = await _unitOfWorks.OrderRepository.GetByIdAsync(id);

        if (order is null) return NotFound();
        
        if (!IsAdmin() && order.CustomerId != GetUserId())
            return Forbid();
        
        if (order.Status != OrderStatus.Pending && !IsAdmin())
            return BadRequest(new { message = "Cannot cancel." });
        
        
        order.Status = OrderStatus.Cancelled;

        await _unitOfWorks.OrderRepository.UpdateAsync(order);
        await _unitOfWorks.SaveChangesAsync();

        return NoContent();
    }
}
