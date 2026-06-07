namespace Delivora.Repositories.Repository;

public class AddressRepository : DelivoraGenericRepository<Address>
{
    public AddressRepository(DeliveryContext context) : base(context)
    {
    }



    public async Task<IEnumerable<Address>> GetByCustomerIdAsync(int customerId)
    {
        return await _dbSet
        .Where(a => a.CustomerId == customerId)
        .ToListAsync();
    }



}
