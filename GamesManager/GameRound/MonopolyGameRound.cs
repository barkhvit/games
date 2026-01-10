using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Millionaire.Core.Interfaces;
using Millionaire.GamesManager.Manager;
using Millionaire.Services.Services;

namespace Millionaire.GamesManager.GameRound
{
    //логика игры монополия
    public class MonopolyGameRound : IGameRound
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IGamesService _gamesService;
        private readonly ILogger<MonopolyGameRound> _logger;
        private readonly IMessageService _messageService;
        public MonopolyGameRound(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _gamesService = _serviceProvider.GetRequiredService<IGamesService>();
            _logger = _serviceProvider.GetRequiredService<ILogger<MonopolyGameRound>>();
            _messageService = _serviceProvider.GetRequiredService<IMessageService>();
        }

        public async Task<bool> IsGameFinished(Guid gameId, CancellationToken ct)
        {
            await Task.CompletedTask; // асинхронная заглушка
            return false; // или true
        }

        public async Task PlayGameRoundAsync(Guid gameId, CancellationToken ct)
        {
            //получаем хранилище запросов к пользователям
            using var scope = _serviceProvider.CreateScope();
            var requests = scope.ServiceProvider.GetRequiredService<RequestsToUsers>();

            //получаем игру
            var game = await _gamesService.GetByIdAsync(gameId, ct);

            //отправляем запрос администратору игры
            if (game != null)
            {
                var AdminId = game.User.Id;

                // 1. Создается "обещание" (TaskCompletionSource)
                var tcs = new TaskCompletionSource<string>();

                // 2. Создается уникальный ключ для этого ожидания
                var key = Guid.NewGuid();

                // 3. Обещание сохраняется в словарь
                requests.Add(key, tcs);
                try
                {
                    // 4. Запрос администратору
                    await _messageService.SendRequestAsync(AdminId, key, ct);

                    // 5. Запускаем процесс ожидания ответа с таймаутом 30 сек
                    using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                    timeoutCts.CancelAfter(TimeSpan.FromSeconds(5));
                    var request = await tcs.Task.WaitAsync(timeoutCts.Token);

                    // 6. Выводим ответ
                    _logger.LogInformation($"Игрок {AdminId} выбрал: {request}");
                }
                catch(OperationCanceledException) when (!ct.IsCancellationRequested)
                {
                    // Только таймаут (не отмена основного токена)
                    _logger.LogWarning($"Игрок {AdminId} не ответил за 5 секунд");
                }
                catch (Exception ex)
                {
                    // Другие ошибки
                    _logger.LogError(ex, $"Ошибка при ожидании ответа от {AdminId}");
                }
                finally
                {
                    // ВСЕГДА удаляем запрос из словаря, чтобы избежать утечек памяти
                    requests.Remove(key);

                    // Отменяем TaskCompletionSource, если он еще ожидается
                    if (!tcs.Task.IsCompleted)
                    {
                        tcs.TrySetCanceled();
                    }
                }
            }

            //в зависимости от ответа делаем действие

            await Task.CompletedTask; // асинхронная заглушка
        }

    }
}
