using Millionaire.Core.DTOs;
using Millionaire.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

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
        public async Task SendMessage(Guid Id, string Text, CancellationToken ct)
        {
            var user = await _usersService.GetByIdAsync(Id, ct);

            if (user != null)
                await _botClient.SendMessage(user.TelegramId, Text, cancellationToken: ct);
        }

    }
}
