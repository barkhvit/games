using Microsoft.Extensions.Logging;
using Millionaire.Core.DTOs;
using Millionaire.GamesManager.Enums;
using Millionaire.GamesManager.Manager;
using Millionaire.TelegramBot.Views.Games;
using Millionaire.TelegramBot.Views.Monopoly;
using Millionaire.Views.MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Millionaire.TelegramBot.Handlers
{
    public class CallbackBotHandler : BaseHandler, IUpdateHandler
    {
        private readonly ILogger<CallbackBotHandler> _logger;
        private readonly MainMenuView _mainMenuView;
        private readonly GamesMainMenuView _gamesMainMenuView;
        private readonly MonopolyMainMenuView _monopolyMainMenuView;
        private readonly GameFabric _gameFabric;

        public CallbackBotHandler(ILogger<CallbackBotHandler> logger, MainMenuView mainMenuView,
            GamesMainMenuView gamesMainMenuView, MonopolyMainMenuView monopolyMainMenuView,
            GameFabric gameFabric)
        {
            _logger = logger;
            _mainMenuView = mainMenuView;
            _gamesMainMenuView = gamesMainMenuView;
            _monopolyMainMenuView = monopolyMainMenuView;
            _gameFabric = gameFabric;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            try
            {
                if (!InitializeMessageInfo(update))
                {
                    _logger.LogInformation($"ошибка инициализации update в {nameof(this.GetType)}");
                    return;
                }

                var callBackDto = new CallBackDto("", "");
                if (Text != null)
                    callBackDto = CallBackDto.FromString(Text);

                var dto = Dto.FromTelegramUpdate(update);

                switch (callBackDto.Object)
                {
                    case nameof(Dto_Objects.MainMenuView): await _mainMenuView.Show(update, ct); break;
                    case nameof(Dto_Objects.GamesMainMenuView): await _gamesMainMenuView.Show(update, ct); break;
                    case nameof(Dto_Objects.MonopolyMainMenuView): await _monopolyMainMenuView.Show(update, ct); break;
                    case nameof(Dto_Objects.CreateGameCommands): await _gameFabric.CreateAsync(callBackDto.Action, dto, ct); break;

                    default: _logger.LogInformation($"Нет подходящего обработчика в CallbackBotHandler.HandleUpdateAsync для {Text}"); break;
                }

                await Task.CompletedTask;
            }
            catch(Exception ex)
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
