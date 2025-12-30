using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Millionaire.Core.DTOs
{
    public class Dto
    {
        public Update? telegramUpdate { get; set; }

        public static Dto FromTelegramUpdate(Update update)
        {
            return new Dto()
            {
                telegramUpdate = update
            };
        }
    }
}
