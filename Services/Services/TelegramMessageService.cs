using Millionaire.Core.DTOs;
using Millionaire.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;

namespace Millionaire.Services.Services
{
    public class TelegramMessageService : IMessageService
    {
        private readonly IUsersService _usersService;
        private readonly ITelegramBotClient _botClient;
        public TelegramMessageService(IUsersService usersService, ITelegramBotClient botClient)
        {
            _usersService = usersService;
            _botClient = botClient;
        }
        public async Task SendMessageAsync(Guid Id, string Text, CancellationToken ct)
        {
            var user = await _usersService.GetByIdAsync(Id, ct);

            if (user != null)
                await _botClient.SendMessage(user.TelegramId, Text, cancellationToken: ct);
        }

        public async Task SendRequestAsync(Guid Id, Guid key, CancellationToken ct)
        {
            var user = await _usersService.GetByIdAsync(Id, ct);

            var buttons = new List<List<InlineKeyboardButton>>();

            buttons.Add(new()
            {
                InlineKeyboardButton.WithCallbackData("Go",new CallBackDto(Dto_Objects.Request, Dto_Action.Go, _id: key).ToString()),
                InlineKeyboardButton.WithCallbackData("Stop",new CallBackDto(Dto_Objects.Request, Dto_Action.Stop, _id: key).ToString())
            });

            if (user != null)
                await _botClient.SendMessage(user.TelegramId, "Выберите действие:", cancellationToken: ct,
                    replyMarkup: new InlineKeyboardMarkup(buttons));
        }
    }
}
