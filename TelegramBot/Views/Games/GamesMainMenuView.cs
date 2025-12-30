using Microsoft.Extensions.Logging;
using Millionaire.Core.DTOs;
using Millionaire.TelegramBot.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Millionaire.TelegramBot.Views.Games
{
    public class GamesMainMenuView : BaseView
    {
        private readonly ILogger<GamesMainMenuView> _logger;
        public GamesMainMenuView(ITelegramBotClient botClient, ILogger<GamesMainMenuView> logger) : base(botClient)
        {
            _logger = logger;
        }

        public override async Task Show(Update update, CancellationToken ct, TypeMessage messageType = TypeMessage.defaultMessage, string inputDto = "")
        {
            try
            {
                if (!InitializeMessageInfo(update))
                {
                    _logger.LogInformation($"ошибка инициализации в GamesMainMenuView");
                    return;
                }

                var buttons = new List<List<InlineKeyboardButton>>();

                buttons.Add(new()
                {
                    InlineKeyboardButton.WithCallbackData("💵 МОНОПОЛИЯ ",new CallBackDto(Dto_Objects.MonopolyMainMenuView, Dto_Action.Show).ToString())
                });
                buttons.Add(new()
                {
                    InlineKeyboardButton.WithCallbackData("⬅️ назад ",new CallBackDto(Dto_Objects.MainMenuView, Dto_Action.Show).ToString())
                });

                if (update.CallbackQuery != null)
                    await _botClient.AnswerCallbackQuery(update.CallbackQuery.Id, cancellationToken: ct);

                await _botClient.EditMessageText(ChatId, MessageId, "🎮 Games: ", cancellationToken: ct,
                    replyMarkup: new InlineKeyboardMarkup(buttons));
            }
            catch
            {
                _logger.LogInformation($"ошибка в GamesMainMenuView");
            }
        }
    }
}
