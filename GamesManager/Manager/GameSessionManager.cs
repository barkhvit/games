using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        
        public GameSessionManager(IServiceProvider serviceProvider,ILogger<GameSessionManager> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _sessions = new ConcurrentDictionary<Guid, (GameSession, Task)>();
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

        public Guid StartNewSession(enNamesOfGames namesOfGames)
        {
            var sessionId = Guid.NewGuid();

            var session = new GameSession(sessionId,
                _serviceProvider.GetRequiredService<ILogger<GameSession>>(), namesOfGames);

            // Запускаем сессию в фоновом режиме
            var sessionTask = session.StartAsync(CancellationToken.None);

            _sessions.TryAdd(sessionId, (session, sessionTask));
            _logger.LogInformation($"Создана новая игровая сессия: {sessionId}");

            return sessionId;
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
