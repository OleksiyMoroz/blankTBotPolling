using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Services;

namespace src.Handlers
{
    public class VideosHandler : ITextMessageHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<PlayersHandler> _logger;
        private readonly ApiService _apiService;
        private readonly ParseService _parseService;
        private readonly UsersStateService _usersStateService;
        private readonly CommonService _commonService;
        private readonly ApplicationDbContext _context;
        public VideosHandler(
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
        public async Task BotOnMessageReceived(string text, long chatId, ITelegramBotClient client)
        {
            await OnShowVideos(text, chatId, client);
            await OnVideoSave(text, chatId, client);
            await OnAddVideoUrl(text, chatId, client);
            await OnAddVideoTitle(text, chatId, client);
        }

        private async Task OnAddVideoTitle(string text, long chatId, ITelegramBotClient client) {
            if(text != Commands.AddFavVideo) return;

            await _commonService.SendTextMessageAsync(chatId, "Please enter the video title:", client);

            var usr = _usersStateService.GetUser(chatId);
            usr.Step = Actions.TypeVideoTitle;
        }

        private async Task OnAddVideoUrl(string text, long chatId, ITelegramBotClient client) {
            var usr = _usersStateService.GetUser(chatId);
            if(usr.Step != Actions.TypeVideoTitle) return;

            usr.VideoTitle = text;

            await _commonService.SendTextMessageAsync(chatId, "Got it!\nPlease enter the video url:", client);

            usr.Step = Actions.TypeVideoSite;
        }

        private async Task OnVideoSave(string text, long chatId, ITelegramBotClient client) {
            var usr = _usersStateService.GetUser(chatId);
            if(usr.Step != Actions.TypeVideoSite) return;

            usr.VideoSite = text;

            var favoriteVideo = new FavoriteVideo {
                ChatId = chatId,
                VideoTitle = usr.VideoTitle,
                VideoUrl = usr.VideoSite
            };

            _context.FavoriteVideos.Add(favoriteVideo);
            await _context.SaveChangesAsync();

            await _commonService.SendTextMessageAsync(chatId, "Video added to favorites!", client);

            usr.Step = Actions.None;
        }

        private async Task OnShowVideos(string text, long chatId, ITelegramBotClient client) {
            if(text != Commands.ViewVideos) return;

            var videos = await _context.FavoriteVideos.ToListAsync();
            if(videos is null || videos.Count == 0) {
                await _commonService.SendTextMessageAsync(chatId, "No saved videos for now", client);
                return;
            }

            var response = _commonService.GetFormattedVideos(videos);

            await _commonService.SendTextMessageAsync(chatId, response, client);
        }
    }
}