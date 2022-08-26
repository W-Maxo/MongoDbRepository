using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Classes;

public interface IRepository<TEntity> where TEntity : EntityBase
{
    Task<List<TEntity>> GetAsync();
    Task<TEntity> GetAsync(string id);
    Task CreateAsync(TEntity entity);
    Task CreateAsync(List<TEntity> entity);
    Task ClearAndCreateAsync(List<TEntity> entities);
    Task UpdateAsync(string id, TEntity updatedEntity);
    Task RemoveAsync();
    Task RemoveAsync(string id);
}