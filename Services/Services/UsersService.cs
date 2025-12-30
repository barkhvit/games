using Millionaire.Core.Enteties;
using Millionaire.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Services.Services
{
    public class UsersService : BaseService<Users, IUsersRepository, Guid>, IUsersService
    {
        public UsersService(IUsersRepository repository) : base(repository)
        {
        }

        public async Task<Users?> AddOrUpdateAsync(Users user, CancellationToken ct)
        {
            var userFromBase = await _repository.GetByTelegramIdAsync(user.TelegramId, ct);

            // если в базе нет такого пользователя c TelegramId, то добавляем
            if(userFromBase == null)
            {
                var n = await _repository.AddAsync(user, ct);
                return user;
            }

            // если есть, то обновляем
            userFromBase.LastVisited = DateTime.UtcNow;
            await _repository.UpdateAsync(userFromBase, ct);
            return userFromBase;
        }

        // GET
        public async Task<Users?> GetByTelegramIdAsync(long id, CancellationToken ct)
        {
            return await _repository.GetByTelegramIdAsync(id, ct);
        }
    }
}
