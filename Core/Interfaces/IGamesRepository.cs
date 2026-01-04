using Millionaire.Core.Enteties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Core.Interfaces
{
    public interface IGamesRepository : IBaseRepository<Games, Guid>
    {
        Task<IReadOnlyList<Games>?> GetByActiveAsync(bool active, CancellationToken ct);
    }
}
