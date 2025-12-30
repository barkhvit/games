using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Millionaire.TelegramBot.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Millionaire.BackGroundServices
{
    public class BotBackgroundService : BackgroundService
    {
        private readonly ILogger<BotBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly CancellationTokenSource _cts;

        public BotBackgroundService(ILogger<BotBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _cts = new CancellationTokenSource();
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Запустился BotBackgroundService");

            using(var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
                    var mainHandler = scope.ServiceProvider.GetRequiredService<MainHandler>();

                    // Запускаем прослушку бота
                    botClient.StartReceiving
                    (
                        updateHandler: mainHandler.HandleUpdateAsync,
                        errorHandler: mainHandler.HandleErrorAsync,
                        cancellationToken: _cts.Token
                    );

                    _logger.LogInformation("Бот запущен");

                    // Ждем сигнала остановки
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        await Task.Delay(1000, stoppingToken);
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Ошибка в BotBackgroundService");
                    throw;
                }
            }
        }
    }
}
