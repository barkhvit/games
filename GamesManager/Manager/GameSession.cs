using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Millionaire.Core.Enteties;
using Millionaire.Core.Interfaces;
using Millionaire.GamesManager.Enums;
using Millionaire.GamesManager.GameRound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.GamesManager.Manager
{
    public class GameSession : BackgroundService
    {
        private readonly ILogger<GameSession> _logger;
        private readonly Guid _id;
        private readonly CancellationTokenSource _cts;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGameRound _gameRound;
        private readonly Games _games;

        public GameSession(Games games, ILogger<GameSession> logger, IServiceProvider serviceProvider)
        {
            _games = games;
            _id = games.Id;
            _logger = logger;
            _cts = new CancellationTokenSource();
            _serviceProvider = serviceProvider;

            _gameRound = _games.TypeOfGame switch
            {
                enNamesOfGames.Monopoly => new MonopolyGameRound(_serviceProvider),
                enNamesOfGames.SeaBattle => new SeaBattleGameRound(_serviceProvider),
                _ => new MonopolyGameRound(_serviceProvider)
            };
        }
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            _logger.LogInformation($"Игра |{_games.Name}| началась. ID-{_id}");

            try
            {
                // Основной игровой цикл
                while (!ct.IsCancellationRequested && !_cts.Token.IsCancellationRequested)
                {
                    // Раунд игры (логика)
                    await _gameRound.PlayGameRoundAsync(_id, _cts.Token);

                    // Проверка условий окончания игры
                    var gameIsFinished = await _gameRound.IsGameFinished(_id, _cts.Token);
                    if (gameIsFinished)
                    {
                        break;
                    }
                    await Task.Delay(100, ct); // Пауза между ходами
                }
            }
            catch
            {

            }
            finally
            {
                _logger.LogInformation($"Сессия {_id} завершена", _id);
                await CleanupAsync();
            }
        }

        private async Task CleanupAsync()
        {
            // Очистка ресурсов сессии
            await Task.CompletedTask;
        }

        public void StopSession()
        {
            _cts.Cancel();
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            StopSession();
            await base.StopAsync(cancellationToken);
        }
    }
}
