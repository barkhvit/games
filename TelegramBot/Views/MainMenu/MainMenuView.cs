
using Telegram.Bot.Types.Enums;
using Millionaire.Core.DTOs;
using Millionaire.TelegramBot;
using Millionaire.TelegramBot.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Millionaire.TelegramBot.Enums;
using Microsoft.Extensions.Logging;

namespace Millionaire.Views.MainMenu
{
    public class MainMenuView : BaseView
    {
        //Главное меню
        private readonly ILogger<MainMenuView> _logger;
        public MainMenuView(ITelegramBotClient botClient, ILogger<MainMenuView> logger) : base(botClient) 
        {
            _logger = logger;
        }

        public override async Task Show(Update update, CancellationToken ct, 
            TypeMessage messageType = TypeMessage.defaultMessage, string inputDto = "")
        {
            //получаем ChatId, UserId, MessageId, Text, User
            if (!InitializeMessageInfo(update))
            {
                _logger.LogInformation("Ошибка при инициализации членов базового класса");
                return;
            }

            //если это нажатие cbq кнопки
            if (update.Type == UpdateType.CallbackQuery && messageType != TypeMessage.newMessage)
            {
                if(update.CallbackQuery != null)
                {
                    await _botClient.AnswerCallbackQuery(update.CallbackQuery.Id, cancellationToken: ct);
                    await _botClient.EditMessageText(ChatId, MessageId, "🏠 Главное меню: ",
                        replyMarkup: MainMenu(), cancellationToken: ct);
                    return;
                }
            }
            if (update.CallbackQuery != null) 
                await _botClient.AnswerCallbackQuery(update.CallbackQuery.Id, cancellationToken: ct);

            await _botClient.SendMessage(ChatId, "🏠 Главное меню: ",
                    replyMarkup: MainMenu(), cancellationToken: ct);
        }

        private static InlineKeyboardMarkup MainMenu()
        {
            List<List<InlineKeyboardButton>> buttons = new()
            {
                new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData("🎮 Games ", new CallBackDto(Dto_Objects.GamesMainMenuView, Dto_Action.Show).ToString())
                }
            };
            return new InlineKeyboardMarkup(buttons);
        }

    }
}
