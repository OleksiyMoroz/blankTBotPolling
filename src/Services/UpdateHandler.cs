using src.Data;
using src.Handlers;
using src.Services;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Telegram.Bot.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<UpdateHandler> _logger;
    private readonly ApplicationDbContext _context;
    private readonly ApiService _apiService;
    private readonly ParseService _parseService;
    private readonly UsersStateService _usersStateService;
    private readonly IServiceProvider _serviceProvider;
    private readonly CommonService _commonService;

    public UpdateHandler(
        ITelegramBotClient botClient, 
        ILogger<UpdateHandler> logger, 
        ApplicationDbContext context, 
        ApiService apiService, 
        ParseService parseService,
        UsersStateService usersStateService,
        IServiceProvider serviceProvider,
        CommonService commonService)
    {
        _botClient = botClient;
        _logger = logger;
        _context = context;
        _apiService = apiService;
        _parseService = parseService;
        _usersStateService = usersStateService;
        _serviceProvider = serviceProvider;
        _commonService = commonService;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { Message: { } message }                       => BotOnMessageReceived(message, cancellationToken),
            _                                              => UnknownUpdateHandlerAsync(update, cancellationToken)
        };

        await handler;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {    
        var chatId = message.Chat.Id;
        var text = message.Text;

        if(string.IsNullOrEmpty(text)) return;

        // welcome message
        if (message.Text == "/start")
        {
            var welcomeText = Commands.MainMenuMessage;
            var keyboard = Commands.GetMainMenuKeyboard();

            await _commonService.SendTextMessageAsync(chatId, welcomeText, _botClient, keyboard);

            return;
        }

        // reset on main menu commands
        if(Commands.IsMainMenuCommand(text)) {
            _usersStateService.ResetUser(chatId);
        }

        // proceed all other services
        foreach(var s in _serviceProvider.GetServices<ITextMessageHandler>())
                await s.BotOnMessageReceived(text, chatId, _botClient);
    }
    private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
#pragma warning restore RCS1163 // Unused parameter.
#pragma warning restore IDE0060 // Remove unused parameter
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);

        // Cooldown in case of network connection error
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }
}