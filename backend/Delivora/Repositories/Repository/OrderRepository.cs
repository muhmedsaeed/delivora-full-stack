namespace Delivora.Repositories.Repository;

public class OrderRepository : DelivoraGenericRepository<Order>
{
    public OrderRepository(DeliveryContext context) : base(context)
    {
    }



    public async Task<IEnumerable<Order>> GetAllWithDetailsAsync()
    {
        return await _dbSet
        .Include(o => o.OrderItems).ThenInclude(i => i.Food)
        .Include(o => o.Address)
        .Include(o => o.Customer).ThenInclude(c => c.User)
        .Include(o => o.Driver!).ThenInclude(d => d.User)
        .Include(o => o.Payment).ThenInclude(p => p.PaymentMethod)
        .OrderByDescending(o => o.CreatedAt)
        .ToListAsync();
    }


    public async Task<Order?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
        .Include(o => o.OrderItems).ThenInclude(i => i.Food)
        .Include(o => o.Address)
        .Include(o => o.Customer).ThenInclude(c => c.User)
        .Include(o => o.Driver!).ThenInclude(d => d.User)
        .Include(o => o.Payment).ThenInclude(p => p.PaymentMethod)
        .FirstOrDefaultAsync(o => o.Id == id);
    }


    public async Task<IEnumerable<Order>> GetByCustomerIdAsync(
    int customerId)
    {
        return await _dbSet
        .Include(o => o.OrderItems).ThenInclude(i => i.Food)
        .Where(o => o.CustomerId == customerId)
        .OrderByDescending(o => o.CreatedAt)
        .ToListAsync();
    }


    public async Task<IEnumerable<Order>> GetByDriverIdAsync(
    int driverId)
    {
        return await _dbSet
        .Include(o => o.OrderItems).ThenInclude(i => i.Food)
        .Where(o => o.DriverId == driverId)
        .OrderByDescending(o => o.CreatedAt)
        .ToListAsync();
    }


}
