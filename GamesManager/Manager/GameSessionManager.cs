using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Millionaire.Core.Enteties;
using Millionaire.Core.Interfaces;
using Millionaire.GamesManager.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.GamesManager.Manager
{
    public class GameSessionManager : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<GameSessionManager> _logger;
        private readonly ConcurrentDictionary<Guid, (GameSession session, Task task)> _sessions;
        private readonly IGamesService _gamesService;
        
        public GameSessionManager(IServiceProvider serviceProvider,ILogger<GameSessionManager> logger,
            IGamesService gamesService)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _sessions = new ConcurrentDictionary<Guid, (GameSession, Task)>();
            _gamesService = gamesService;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Менеджер игровых сессий запущен");
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Останавливаем все активные сессии
            var stopTasks = _sessions.Values.Select(v => v.session.StopAsync(cancellationToken));
            await Task.WhenAll(stopTasks);

            _logger.LogInformation("Менеджер игровых сессий остановлен");
        }

        public Guid StartNewSession(Games games)
        {
            //проверить в словаре, может игра уже запущена
            if (!_sessions.TryGetValue(games.Id, out var sessionData))
            {
                //если нет, то создаем сессию
                var session = new GameSession(games, _serviceProvider.GetRequiredService<ILogger<GameSession>>()
                    , _serviceProvider);

                // Запускаем сессию в фоновом режиме
                var sessionTask = session.StartAsync(CancellationToken.None);

                //добавляется в словарь
                _sessions.TryAdd(games.Id, (session, sessionTask));
                _logger.LogInformation($"Добавлена в память новая игровая сессия: {games.Name}");
            }
            return games.Id;
        }

        public async Task StopSessionAsync(Guid sessionId)
        {
            if (_sessions.TryRemove(sessionId, out var sessionInfo))
            {
                await sessionInfo.session.StopAsync(CancellationToken.None);
                await sessionInfo.task; // Ждем завершения задачи
            }
        }

        public IEnumerable<Guid> GetActiveSessions()
        {
            return _sessions.Keys;
        }
    }
}
