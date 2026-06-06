namespace Delivora.Repositories.Repository;

public class CategoryRepository : DelivoraGenericRepository<Category>
{
    public CategoryRepository(DeliveryContext context) : base(context)
    {
    }


    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }


    public async Task<IEnumerable<Category>> GetAllWithFoodsAsync()
    {
        return await _dbSet.Include(c => c.Foods).ToListAsync();
    }


}
