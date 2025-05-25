using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Handlers;
using src.Services;
using Telegram.Bot.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var conf = context.Configuration;

        services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    var token = conf.GetSection("BOT_TOKEN");
                    TelegramBotClientOptions options = new(token.Value ?? throw new Exception("BOT_TOKEN is not valid"));
                    return new TelegramBotClient(options, httpClient);
                });


        services.AddScoped<UpdateHandler>();
        services.AddScoped<ReceiverService>();
        services.AddHostedService<PollingService>();
        services.AddDbContext<ApplicationDbContext>(options => {
            var connectionString = conf.GetSection("CONNECTION_STRING");
            options.UseSqlite(connectionString.Value);
        });

        services.AddHttpClient("api", httpClient =>
        {
            var baseUrl = conf.GetSection("RAPID_BASE_URL");
            var apiKey = conf.GetSection("API_KEY");

            httpClient.BaseAddress = new Uri(baseUrl.Value ?? throw new Exception("RAPID_BASEURL is not valid"));
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", apiKey.Value ?? throw new Exception("API_KEY is not valid"));
            httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", "free-nba.p.rapidapi.com");

        }).SetHandlerLifetime(TimeSpan.FromMinutes(10));

        services.AddScoped<ApiService>();

        services.AddSingleton<ParseService>();
        services.AddSingleton<UsersStateService>();

        services.AddScoped<CommonService>();
        services.AddScoped<ITextMessageHandler, GamesAndStatsHandler>();
        services.AddScoped<ITextMessageHandler, PlayersHandler>();
        services.AddScoped<ITextMessageHandler, RemindHandler>();
        services.AddScoped<ITextMessageHandler, TeamsHandler>();
        services.AddScoped<ITextMessageHandler, VideosHandler>();

        services.AddHostedService<RemindBackgroundService>();
    })
    .Build();

var scope = host.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
try {
    context.Database.EnsureCreated();
}
catch {}

await host.RunAsync();