using learning_center_webapi.Contexts.Shared.Domain.Repositories;
using learning_center_webapi.Contexts.Shared.Infraestructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace learning_center_webapi.Contexts.Shared.Infraestructure.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    private readonly LearningCenterContext _context;

    public BaseRepository(LearningCenterContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TEntity entity)
    {
        await _context.AddAsync(entity); //'insert into table value("'
    }

    public async Task<TEntity?> FindByIdAsync(int id)
    {
        // 'select * from table where id = ...'
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity); //'update table set ... where id = ...'
    }

    public void Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity); //'delete from table where id = ...'
    }

    public async Task<IEnumerable<TEntity>> ListAsync()
    {
        // 'select * from table'
        return await _context.Set<TEntity>().ToListAsync();
    }
}