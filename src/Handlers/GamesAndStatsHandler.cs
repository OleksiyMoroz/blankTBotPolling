using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Services;

namespace src.Handlers
{
    public class GamesAndStatsHandler : ITextMessageHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<PlayersHandler> _logger;
        private readonly ApiService _apiService;
        private readonly ParseService _parseService;
        private readonly UsersStateService _usersStateService;
        private readonly CommonService _commonService;
        public GamesAndStatsHandler(
            ITelegramBotClient botClient, 
            ILogger<PlayersHandler> logger,         
            ApiService apiService, 
            ParseService parseService,
            UsersStateService usersStateService,
            CommonService commonService)
    {
        _botClient = botClient;
        _logger = logger;
        _apiService = apiService;
        _parseService = parseService;
        _usersStateService = usersStateService;
        _commonService = commonService;

    }
        public async Task BotOnMessageReceived(string text, long chatId, ITelegramBotClient client)
        {
            await OnGames(text, chatId, client);
            await OnStats(text, chatId, client);
        }

        private async Task OnStats(string text, long chatId, ITelegramBotClient client) {
            if(text != Commands.Stats) return;

            var response = await _apiService.GetApiResponse("stats?page=0&per_page=25");
            var stats = _parseService.DataToStats(response);
            var formatted = _commonService.GetFormattedStats(stats);

            await _commonService.SendTextMessageAsync(chatId, formatted, client);
        }

        private async Task OnGames(string text, long chatId, ITelegramBotClient client) {
            if(text != Commands.AllGames) return;

            var response = await _apiService.GetApiResponse("games?page=0&per_page=25");
            var games = _parseService.DataToGames(response);
            var formatted = _commonService.GetFormattedGames(games);

            await _commonService.SendTextMessageAsync(chatId, formatted, client);
        }
    }
}