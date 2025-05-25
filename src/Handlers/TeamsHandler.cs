using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Services;

namespace src.Handlers
{
    public class TeamsHandler : ITextMessageHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<PlayersHandler> _logger;
        private readonly ApiService _apiService;
        private readonly ParseService _parseService;
        private readonly UsersStateService _usersStateService;
        private readonly CommonService _commonService;
        public TeamsHandler(
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
            await OnSpecificTeamResponse(text, chatId, client);
            await OnSpecificTeam(text, chatId, client);
        }

        private async Task OnSpecificTeam(string text, long chatId, ITelegramBotClient client) {
            if(text != Commands.SpecificTeam) return;

            await _commonService.SendTextMessageAsync(chatId, "Please enter the team's ID:", client);

            var usr = _usersStateService.GetUser(chatId);
            usr.Step = Actions.TypeTeamId;
        }

        private async Task OnSpecificTeamResponse(string text, long chatId, ITelegramBotClient client) {
            var usr = _usersStateService.GetUser(chatId);
            if(usr.Step != Actions.TypeTeamId) return;

            if(int.TryParse(text, out int teamId)) {
                var response = await _apiService.GetApiResponse("teams/" + teamId);
                    if(string.IsNullOrEmpty(response)) {
                    await _commonService.SendTextMessageAsync(chatId, "Not found", client);
                    return;
                }
                
                var team = _parseService.DataToTeam(response);
                var formatted = _commonService.GetFormattedTeam(team);

                await _commonService.SendTextMessageAsync(chatId, formatted, client);
            }
            else {
                await _commonService.SendTextMessageAsync(chatId, "Team ID are invalid", client);
            }
        }
    }
}