using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Millionaire.Core.Interfaces;
using Millionaire.GamesManager.Manager;

namespace Millionaire.BackGroundServices
{
    //задача сервиса:
    //получаем активные игры - проверяем в хранилище GamesessionManager - если нет, то запускаем игровую сессию (добавляем в хранилище)
    //получаем неактивные игры - если есть в хранилище, то останавливаем (удаляем из хранилища)
    internal class GamesManagerBackGroundService : BackgroundService
    {
        private readonly IGamesService _gamesService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<GamesManagerBackGroundService> _logger;
        public GamesManagerBackGroundService(IGamesService gamesService, IServiceProvider serviceProvider,
            ILogger<GamesManagerBackGroundService> logger)
        {
            _gamesService = gamesService;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            _logger.LogInformation("Запустился GamesManagerBackGroundService");

            using var scope = _serviceProvider.CreateScope();
            var sessionManager = scope.ServiceProvider.GetRequiredService<GameSessionManager>();

            try
            {
                // Ждем сигнала остановки
                while (!ct.IsCancellationRequested)
                {
                    //получаем и запускаем активные игры
                    var activeGames = await _gamesService.GetByActiveAsync(true, ct);

                    if (activeGames != null)
                    {
                        foreach (var game in activeGames)
                        {
                            sessionManager.StartNewSession(game);
                        }
                    }
                    await Task.Delay(2000, ct);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"ошибка в {_logger.ToString()}");
            }
        }
    }
}
