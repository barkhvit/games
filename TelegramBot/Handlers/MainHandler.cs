using Microsoft.Extensions.Logging;
using Millionaire.BackGroundServices;
using Millionaire.Core.Enteties;
using Millionaire.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Millionaire.TelegramBot.Handlers
{
    public class MainHandler : BaseHandler, IUpdateHandler
    {
        private readonly ILogger<BotBackgroundService> _logger;
        private readonly IUsersService _usersService;
        private readonly MessageBotHandler _messageBotHandler;
        private readonly CallbackBotHandler _callbackBotHandler;
        public MainHandler(ILogger<BotBackgroundService> logger, ITelegramBotClient telegramBotClient,
            IUsersService usersService, MessageBotHandler messageBotHandler, CallbackBotHandler callbackBotHandler) 
        {
            _logger = logger;
            _usersService = usersService;
            _messageBotHandler = messageBotHandler;
            _callbackBotHandler = callbackBotHandler;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            try
            {
                // Инициализируем данные, если не смогли инициализировать то сбрасываемся
                if (!InitializeMessageInfo(update))
                {
                    _logger.LogInformation("ошибка инициализации update в MainHandler");
                    return;
                }

                //регистрируем пользователя или обновляем пользователя
                User = await RegistrationOrUpdateUser(update, ct);

                //в зависимости от типа Update используем нужный обработчик
                switch (update.Type)
                {
                    case UpdateType.Message: await _messageBotHandler.HandleUpdateAsync(botClient, update, ct); break;
                    case UpdateType.CallbackQuery: await _callbackBotHandler.HandleUpdateAsync(botClient, update, ct); break;
                }
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"Ошибка обработки сообщения: {ex.Message}");
            }
        }

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken ct)
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

        private async Task<Users?> RegistrationOrUpdateUser(Update update, CancellationToken ct)
        {
            string? username = null;
            if (TelegramUser != null)
                username = TelegramUser.Username;

            var user = new Users()
            {
                Id = Guid.NewGuid(),
                TelegramId = UserId,
                Username = username,
                LastVisited = DateTime.UtcNow
            };

            var u = await _usersService.AddOrUpdateAsync(user, ct);

            if(u!=null)
                _logger.LogInformation($"{u.LastVisited}, user:{u.TelegramId}, текст:{Text}");

            return u;
        }
    }
}
