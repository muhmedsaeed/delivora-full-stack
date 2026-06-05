namespace Delivora.Repositories.Repository;

public class FoodRepository : DelivoraGenericRepository<Food>
{

    public FoodRepository(DeliveryContext context) : base(context)
    {
    }


    public async Task<IEnumerable<Food>> GetFoodsByCategoryIdAsync(int categoryId)
    {
        return await _dbSet.Where(f => f.CategoryId == categoryId).ToListAsync();
    }


    public async Task<Food?> GetFoodByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(f => f.Name.ToLower() == name.ToLower());

    }




}