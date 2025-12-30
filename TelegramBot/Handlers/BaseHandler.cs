using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Diagnostics;

namespace Millionaire.TelegramBot.Handlers
{
    public abstract class BaseHandler
    {
        public long ChatId { get; protected set; }
        public long UserId { get; protected set; }
        public int MessageId { get; protected set; }
        public User? User { get; protected set; } 
        public string? Text { get; protected set; } 


        protected BaseHandler()
        {
        }

        protected (long chatId, long userId, int messageId, string? text, User? user) GetMessageInfo(Update update)
        {
            return update switch
            {
                { Type: UpdateType.Message, Message: var msg }
                    when msg?.From != null && msg.Chat != null
                    => (msg.Chat.Id, msg.From.Id, msg.MessageId, msg.Text, msg.From),

                { Type: UpdateType.EditedMessage, EditedMessage: var msg }
                when msg?.From != null && msg.Chat != null
                => (msg.Chat.Id, msg.From.Id, msg.MessageId, msg.Text, msg.From),

                { Type: UpdateType.CallbackQuery, CallbackQuery: var cbq }
                    when cbq?.From != null && cbq.Message?.Chat != null
                    => (cbq.Message.Chat.Id, cbq.From.Id, cbq.Message.MessageId, cbq.Data, cbq.From),

                _ => (0, 0, 0, null, null)

            };
        }

        protected bool InitializeMessageInfo(Update update)
        {
            try
            {
                (ChatId, UserId, MessageId, Text, User) = GetMessageInfo(update);

                // Проверяем, что получили валидные данные
                return ChatId != 0 && UserId != 0 && User != null;
            }

            catch
            {
                return false;
            }
        }


    }
}