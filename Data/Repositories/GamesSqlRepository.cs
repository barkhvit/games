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
    public class GamesSqlRepository : IGamesRepository
    {
        private readonly IDataContextFactory<DBContext> _factory;

        public GamesSqlRepository(IDataContextFactory<DBContext> factory)
        {
            _factory = factory;
        }

        //ADD (INSERT)
        public async Task<int> AddAsync(Games user, CancellationToken ct)
        {
            using var context = _factory.CreateDataContext();
            return await context.InsertAsync(ModelMapper.MapToModel(user), token: ct);
        }

        // DELETE
        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            using var context = _factory.CreateDataContext();
            var n = await context.GetTable<GamesModel>()
                .Where(g => g.Id == id)
                .DeleteAsync(ct);
            return n > 0;
        }

        // GET (SELECT)
        public async Task<Games?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            using var context = _factory.CreateDataContext();
            var g = await context.games
                .LoadWith(g => g.User)
                .FirstOrDefaultAsync(g => g.Id == id, ct);
            return g == null ? null : ModelMapper.MapFromModel(g);
        }

        // UPDATE
        public async Task<int> UpdateAsync(Games game, CancellationToken ct)
        {
            using var context = _factory.CreateDataContext();
            return await context.UpdateAsync(ModelMapper.MapToModel(game), token: ct);
        }
    }
}
