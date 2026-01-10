using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Millionaire.Core.Interfaces;

namespace Millionaire.GamesManager.GameRound
{
    public interface IGameRound
    {
        Task PlayGameRoundAsync(Guid gameId, CancellationToken ct);
        Task<bool> IsGameFinished(Guid gameId, CancellationToken ct);
    }
}
