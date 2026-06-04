namespace Delivora.Repositories.Repository;

public class DelivoraGenericRepository<TEntity> where TEntity : class
{
    private readonly DeliveryContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public DelivoraGenericRepository(DeliveryContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }


    // ========================= Read =========================
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }



    // ========================= Create =========================
    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        // save changes is handled by the unit of work.
    }



    // ========================= Update =========================
    public async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        // save changes is handled by the unit of work.
    }



    // ========================= Delete =========================
    public async Task DeleteByIdAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            await DeleteAsync(entity);
        }
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        // save changes is handled by the unit of work.
    }


}
