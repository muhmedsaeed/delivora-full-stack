namespace Delivora.Repositories.Repository;

public class FoodReviewRepository: DelivoraGenericRepository<FoodReview>
{
    public FoodReviewRepository(DeliveryContext context) : base(context) { }


    public async Task<List<FoodReview>> GetAllWithCustomerAsync()
    {
        return await _dbSet.Include(c => c.Customer).ToListAsync();
    }
}
