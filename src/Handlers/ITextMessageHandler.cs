using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace src.Handlers
{
    public interface ITextMessageHandler
    {
        Task BotOnMessageReceived(string text, long chatId, ITelegramBotClient client);
    }
}