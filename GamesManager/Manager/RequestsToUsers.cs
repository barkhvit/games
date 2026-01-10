using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Millionaire.GamesManager.Manager
{
    public class RequestsToUsers : IHostedService
    {
        private readonly ConcurrentDictionary<Guid, TaskCompletionSource<string>> _requests = new();
        private readonly ILogger<RequestsToUsers> _logger;
        public RequestsToUsers(ILogger<RequestsToUsers> logger)
        {
            _logger = logger;
        }

        public bool Add(Guid id, TaskCompletionSource<string> newRequest)
        {
            return _requests.TryAdd(id, newRequest);
        }

        public bool Remove(Guid id)
        {
            return _requests.TryRemove(id, out _);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Менеджер запросов к пользователю запущен");
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Менеджер запросов к пользователю завершен");
            await Task.CompletedTask;
        }

        internal async Task AnswerToRequest(Guid? id, string action, CancellationToken ct)
        {
            if(id != null)
            {
                //Ищем ожидающее обещание
                var tcs = _requests[(Guid)id];

                // "Выполняем" обещание
                tcs.TrySetResult(action);

                //Удаляем обещание
                Remove((Guid)id);
            }
            await Task.CompletedTask;
        }
    }
}
