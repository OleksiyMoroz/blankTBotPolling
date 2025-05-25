using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Services;
using Telegram.Bot.Types;

namespace src.Handlers
{
    public class PlayersHandler : ITextMessageHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<PlayersHandler> _logger;
        private readonly ApiService _apiService;
        private readonly ParseService _parseService;
        private readonly UsersStateService _usersStateService;
        private readonly CommonService _commonService;
        public PlayersHandler(
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
            await OnPlayerResponse(text, chatId, client);
            await OnPlayer(text, chatId, client);
            await OnAllPlayers(text, chatId, client);
        }

        private async Task OnAllPlayers(string text, long chatId, ITelegramBotClient client) {
            if(text != Commands.AllPlayers) return;

            var response = await _apiService.GetApiResponse("players?page=0&per_page=25");
            var players = _parseService.DataToPlayers(response);
            var formatted = _commonService.GetFormattedPlayers(players.OrderBy(p => p.Id).ToList());

            await _commonService.SendTextMessageAsync(chatId, formatted, client);
        }

        private async Task OnPlayer(string text, long chatId, ITelegramBotClient client) {
            if(text != Commands.SpecificPlayer) return;

            await _commonService.SendTextMessageAsync(chatId, "Please enter the player's ID:", client);

            var usr = _usersStateService.GetUser(chatId);
            usr.Step = Actions.TypePlayerId;
        }

        private async ValueTask OnPlayerResponse(string text, long chatId, ITelegramBotClient client) {
            var usr = _usersStateService.GetUser(chatId);
            if(usr.Step != Actions.TypePlayerId) return;

            if(!int.TryParse(text, out int playerId)) {
                await _commonService.SendTextMessageAsync(chatId, "Type proper ID", client);
            }

            var response = await _apiService.GetApiResponse($"players/{playerId}");
            if(string.IsNullOrEmpty(response)) {
                await _commonService.SendTextMessageAsync(chatId, "Not found", client);
                return;
            }
            
            var player = _parseService.DataToPlayer(response);
            var formatted = _commonService.GetFormattedPlayer(player);

            await _commonService.SendTextMessageAsync(chatId, formatted, client);
        }
    }
}