using Millionaire.Core.Enteties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Core.Interfaces
{
    public interface IUsersRepository : IBaseRepository<Users, Guid>
    {
        Task<Users?> GetByTelegramIdAsync(long telegramId, CancellationToken ct);
    }
}
