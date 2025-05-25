using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Data;
using src.Services;

namespace src.Handlers
{
    public class RemindHandler : ITextMessageHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<PlayersHandler> _logger;
        private readonly ApiService _apiService;
        private readonly ParseService _parseService;
        private readonly UsersStateService _usersStateService;
        private readonly CommonService _commonService;
        private readonly ApplicationDbContext _context;
        public RemindHandler(
            ITelegramBotClient botClient, 
            ILogger<PlayersHandler> logger,         
            ApiService apiService, 
            ParseService parseService,
            UsersStateService usersStateService,
            CommonService commonService,
            ApplicationDbContext context)
    {
        _botClient = botClient;
        _logger = logger;
        _apiService = apiService;
        _parseService = parseService;
        _usersStateService = usersStateService;
        _commonService = commonService;
        _context = context;

    }
        public async ValueTask BotOnMessageReceived(string text, long chatId, ITelegramBotClient client)
        {
            await OnRemindSave(text, chatId, client);
            await OnAddRemind(text, chatId, client);
        }

        private async ValueTask OnAddRemind(string text, long chatId, ITelegramBotClient client) {
            if(text != Commands.Reminder) return;

            await _commonService.SendTextMessageAsync(chatId, "Please enter the game details and reminder date and time in the following format:\n" +
                "Game details - Date and Time (e.g., Lakers vs. Warriors - 2023-07-26 19:00)", client);

            var usr = _usersStateService.GetUser(chatId);
            usr.Step = Actions.TypindRemind;
        }

        private async ValueTask OnRemindSave(string text, long chatId, ITelegramBotClient client) {
            var usr = _usersStateService.GetUser(chatId);
            if(usr.Step != Actions.TypindRemind) return;

            var split = text.Split(" - ");

            if(split.Count() < 2 || split.First().Count() < 3 || !split.First().Contains("vs")) {
                await SendInvalidRemindMessage(chatId, client);
                return;
            }

            if (DateTime.TryParseExact(split[1], "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime reminderDateTime))
                {
                    if(reminderDateTime < DateTime.Now) {
                        await SendInvalidRemindMessage(chatId, client);
                        return;
                    }
                    
                    var reminder = new Reminder
                    {
                        ChatId = chatId,
                        ReminderDateTime = reminderDateTime,
                        GameDetails = text
                    };

                    _context.Reminders.Add(reminder);
                    await _context.SaveChangesAsync();

                    await _commonService.SendTextMessageAsync(chatId, "Reminder set for the game!", client);
                    
                }
                else
                {
                    await SendInvalidRemindMessage(chatId, client);
                }
        }

        private async Task SendInvalidRemindMessage(long chatId, ITelegramBotClient client) {
            await _commonService.SendTextMessageAsync(chatId, "Invalid date and time format. Please try again.", client);
        }
    }
}