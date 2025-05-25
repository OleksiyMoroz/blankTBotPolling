using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using src.Data;

namespace src.Services
{
    public class RemindBackgroundService : BackgroundService
    {
        private readonly PeriodicTimer _timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
        private IServiceProvider _services;
        private readonly CommonService _commonService;
        private readonly ITelegramBotClient _botClient;
        public RemindBackgroundService(IServiceProvider services, CommonService commonService, ITelegramBotClient botClient) {
            _services = services;
            _commonService = commonService;
            _botClient = botClient;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested) {
                await CheckAndSendRemind();
            }
        }

        private async Task CheckAndSendRemind() {
            var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var reminders = await context.Reminders.ToListAsync();

            var toRemove = new List<Reminder>();

            try {
                foreach(var r in reminders) {
                    if(r.ReminderDateTime >= DateTime.Now) continue;

                    toRemove.Add(r);
                    await _commonService.SendTextMessageAsync(r.ChatId, $"☝️ Reminder ☝️ \n\nGame {r.GameDetails} about to begin!", _botClient);
                }

                    context.RemoveRange(toRemove);
                    await context.SaveChangesAsync();
            }
            catch {}
        }
    }
}