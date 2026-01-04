using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Millionaire.Core.Enteties;
using Millionaire.GamesManager.Enums;
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
        private readonly enNamesOfGames _namesOfGames;

        public GameSession(Games games, ILogger<GameSession> logger)
        {
            _id = games.Id;
            _logger = logger;
            _cts = new CancellationTokenSource();
            _namesOfGames = games.TypeOfGame;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Игра {_id} началась");

            try
            {
                // Основной игровой цикл
                while (!stoppingToken.IsCancellationRequested &&
                       !_cts.Token.IsCancellationRequested)
                {
                    // Логика игры
                    await PlayGameRoundAsync();

                    // Проверка условий окончания игры
                    if (IsGameFinished())
                    {
                        break;
                    }

                    await Task.Delay(100, stoppingToken); // Пауза между ходами
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

        private async Task PlayGameRoundAsync()
        {
            // Ваша игровая логика
            await Task.Delay(50); // Пример
        }

        private bool IsGameFinished()
        {
            // Проверка условий окончания игры
            return false; // Пример
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
