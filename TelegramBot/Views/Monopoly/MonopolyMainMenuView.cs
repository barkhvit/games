using Microsoft.Extensions.Logging;
using Millionaire.Core.DTOs;
using Millionaire.GamesManager.Enums;
using Millionaire.TelegramBot.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Millionaire.TelegramBot.Views.Monopoly
{
    public class MonopolyMainMenuView : BaseView
    {
        private readonly ILogger<MonopolyMainMenuView> _logger;
        public MonopolyMainMenuView(ITelegramBotClient botClient,
            ILogger<MonopolyMainMenuView> logger) : base(botClient)
        {
            _logger = logger;
        }

        public override async Task Show(Update update, CancellationToken ct, TypeMessage messageType = TypeMessage.defaultMessage, string inputDto = "")
        {
            try
            {
                if (!InitializeMessageInfo(update))
                {
                    _logger.LogInformation("Ошибка при инициализации в MonopolyMainMenuView.Show");
                    return;
                }

                if (update.CallbackQuery != null)
                    await _botClient.AnswerCallbackQuery(update.CallbackQuery.Id, cancellationToken: ct);

                var buttons = new List<List<InlineKeyboardButton>>();

                buttons.Add(new()
                {
                    InlineKeyboardButton.WithCallbackData("+ новая игра",new CallBackDto(Dto_Objects.CreateGameCommands, enNamesOfGames.Monopoly.ToString()).ToString()),
                    InlineKeyboardButton.WithCallbackData("⬅️ назад",new CallBackDto(Dto_Objects.GamesMainMenuView,Dto_Action.Show).ToString())
                });

                await _botClient.EditMessageText(ChatId, MessageId, "💵 MONOPOLY 💵", cancellationToken: ct,
                    replyMarkup: new InlineKeyboardMarkup(buttons));
            }
            catch
            {
                _logger.LogInformation("Ошибка в MonopolyMainMenuView.Show");
            }
            
        }
    }
}
