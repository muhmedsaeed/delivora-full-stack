

namespace Delivora.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Customer")]
public class AddressController : ControllerBase
{
    private readonly UnitOfWorks _unitOfWorks;
    private readonly IMapper _mapper;

    public AddressController(UnitOfWorks uow, IMapper mapper)
    {
        _unitOfWorks = uow;
        _mapper = mapper;
    }



    // Helper method to get the current user's ID from the claims
    private int GetUserId()
    {

        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }




    [HttpGet]
    public async Task<IActionResult> GetMyAddresses()
    {
        var list = await _unitOfWorks.AddressRepository.GetByCustomerIdAsync(GetUserId());

        return Ok(_mapper.Map<List<AddressDto>>(list));
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var address = await _unitOfWorks.AddressRepository.GetByIdAsync(id);

        if (address is null || address.CustomerId != GetUserId())
            return NotFound();

        return Ok(_mapper.Map<AddressDto>(address));
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAddressDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var address = _mapper.Map<Address>(dto);

        address.CustomerId = GetUserId();

        await _unitOfWorks.AddressRepository.AddAsync(address);
        await _unitOfWorks.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = address.Id }, _mapper.Map<AddressDto>(address));
    }



    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAddressDto dto)
    {
        var address = await _unitOfWorks.AddressRepository.GetByIdAsync(id);

        if (address is null || address.CustomerId != GetUserId())
            return NotFound();

        _mapper.Map(dto, address);

        await _unitOfWorks.AddressRepository.UpdateAsync(address);
        await _unitOfWorks.SaveChangesAsync();
        return NoContent();
    }



    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var address = await _unitOfWorks.AddressRepository.GetByIdAsync(id);

        if (address is null || address.CustomerId != GetUserId())
            return NotFound();

        await _unitOfWorks.AddressRepository.DeleteAsync(address);
        await _unitOfWorks.SaveChangesAsync();

        return NoContent();
    }


}