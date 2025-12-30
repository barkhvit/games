
using LinqToDB;
using LinqToDB.Async;
using Millionaire.Core.Enteties;
using Millionaire.Core.Interfaces;
using Millionaire.Data.DataContext;
using Millionaire.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Data.Repositories
{
    public class UsersSqlRepository : IUsersRepository
    {
        private readonly IDataContextFactory<DBContext> _factory;

        public UsersSqlRepository(IDataContextFactory<DBContext> factory)
        {
            _factory = factory;
        }

        // ADD (INSERT)
        public async Task<int> AddAsync(Users user, CancellationToken ct)
        {
            using var context = _factory.CreateDataContext();
            return await context.InsertAsync(ModelMapper.MapToModel(user), token: ct);
        }

        // DELETE
        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            using var context = _factory.CreateDataContext();
            var n = await context.GetTable<Users>()
                .Where(u => u.Id == id)
                .DeleteAsync(ct);
            return n > 0;
        }

        // GET (SELECT)
        public async Task<Users?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            using var context = _factory.CreateDataContext();
            var user = await context.GetTable<UserModel>()
                .FirstOrDefaultAsync(u => u.Id == id, ct);
            return user == null ? null : ModelMapper.MapFromModel(user);
        }

        public async Task<Users?> GetByTelegramIdAsync(long telegramId, CancellationToken ct)
        {
            using var context = _factory.CreateDataContext();
            var user = await context.GetTable<UserModel>()
                .FirstOrDefaultAsync(u => u.TelegramId == telegramId, ct);
            return user == null ? null : ModelMapper.MapFromModel(user);
        }

        // UPDATE
        public async Task<int> UpdateAsync(Users user, CancellationToken ct)
        {
            using var context = _factory.CreateDataContext();
            return await context.UpdateAsync(ModelMapper.MapToModel(user),token: ct);
        }
    }
}
