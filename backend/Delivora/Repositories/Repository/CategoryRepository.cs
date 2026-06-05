namespace Delivora.Repositories.Repository;

public class CategoryRepository : DelivoraGenericRepository<Category>
{
    public CategoryRepository(DeliveryContext context) : base(context)
    {
    }


    public async Task<Category?> GetCategoryByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }
}
