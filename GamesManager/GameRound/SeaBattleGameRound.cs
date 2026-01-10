using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.GamesManager.GameRound
{
    public class SeaBattleGameRound : IGameRound
    {
        private readonly IServiceProvider _serviceProvider;
        public SeaBattleGameRound(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<bool> IsGameFinished(Guid gameId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task PlayGameRoundAsync(Guid gameId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
