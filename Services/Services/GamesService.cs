using Millionaire.Core.Enteties;
using Millionaire.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Services.Services
{
    public class GamesService : BaseService<Games, IGamesRepository, Guid>, IGamesService
    {
        public GamesService(IGamesRepository repository) : base(repository)
        {
        }

        public async Task<IReadOnlyList<Games>?> GetByActiveAsync(bool active, CancellationToken ct)
        {
            return await _repository.GetByActiveAsync(active, ct);
        }
    }
}
