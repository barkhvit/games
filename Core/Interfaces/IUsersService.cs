using Millionaire.Core.Enteties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Core.Interfaces
{
    public interface IUsersService : IBaseService<Users, Guid>
    {
        Task<Users?> AddOrUpdateAsync(Users user, CancellationToken ct);

        Task<Users?> GetByTelegramIdAsync(long id, CancellationToken ct);
    }
}
