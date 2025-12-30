using Millionaire.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Services.Services
{
    public abstract class BaseService<TEntity, TRepository, TKey> : IBaseService<TEntity, TKey>
        where TEntity : class
        where TRepository : IBaseRepository<TEntity, TKey>
        
    {
        protected readonly TRepository _repository;
        protected BaseService(TRepository repository)
        {
            _repository = repository;
        }


        public async Task<int> AddAsync(TEntity entity, CancellationToken ct)
        {
            return await _repository.AddAsync(entity, ct);
        }

        public async Task<bool> DeleteAsync(TKey id, CancellationToken ct)
        {
            return await _repository.DeleteAsync(id, ct);
        }

        public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct)
        {
            return await _repository.GetByIdAsync(id, ct);
        }

        public async Task<int> UpdateAsync(TEntity entity, CancellationToken ct)
        {
            return await _repository.UpdateAsync(entity, ct);
        }
    }
}
