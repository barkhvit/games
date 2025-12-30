using Microsoft.Extensions.Logging;
using Millionaire.Core.DTOs;
using Millionaire.Views.MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Millionaire.TelegramBot.Handlers
{
    public class MessageBotHandler : BaseHandler, IUpdateHandler
    {
        private readonly ILogger<MessageBotHandler> _logger;
        private readonly MainMenuView _mainMenuView;
        public MessageBotHandler(ILogger<MessageBotHandler> logger, MainMenuView mainMenuView)
        {
            _logger = logger;
            _mainMenuView = mainMenuView;
        }
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            try
            {
                // Инициализируем данные, если не смогли инициализировать, то сбрасываемся
                if (!InitializeMessageInfo(update))
                {
                    _logger.LogInformation($"ошибка инициализации update в {nameof(this.GetType)}");
                    return;
                }

                var dto = new CallBackDto("", "");
                if (Text != null)
                    dto = CallBackDto.FromString(Text);

                switch(Text)
                {
                    case "/start":
                        await _mainMenuView.Show(update, ct); break;
                }


                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Ошибка обработки сообщения: {ex.Message}");
            }
        }

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Ошибка в боте:");
            _logger.LogInformation($"Источник: {source}");
            _logger.LogInformation($"Сообщение: {exception.Message}");

            // Для критических ошибок можно уведомить администратора
            if (exception is ApiRequestException apiEx && apiEx.ErrorCode == 429)
            {
                _logger.LogInformation("Превышен лимит запросов!");
            }

            await Task.CompletedTask;
        }
    }
}
