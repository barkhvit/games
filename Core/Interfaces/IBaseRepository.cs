using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Core.Interfaces
{
    public interface IBaseRepository<TEntity, TKey> where TEntity : class
    {
        //GET (SELECT)
        Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct);

        //DELETE
        Task<bool> DeleteAsync(TKey id, CancellationToken ct);

        //UPDATE
        Task<int> UpdateAsync(TEntity entity, CancellationToken ct);

        //ADD (INSERT)
        Task<int> AddAsync(TEntity entity, CancellationToken ct);
    }
}
