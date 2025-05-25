using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Data;
using src.Models;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace src.Services
{
    public class CommonService
    {
        public async Task SendTextMessageAsync(long chatId, string text, ITelegramBotClient client, ReplyMarkup replyMarkup = null)
        {
            if (text.Length > 4096)
            {
                for (int i = 0; i < text.Length / 4096; i++)
                {
                    string chunk = text.Substring(i * 4096, 4096);
                    await  client.SendMessage(chatId, chunk, replyMarkup: replyMarkup, parseMode: ParseMode.Html);
                }
            }
            else
                await client.SendMessage(chatId, text, replyMarkup: replyMarkup, parseMode: ParseMode.Html);
        }

        public string GetFormattedPlayers(List<Player> players) {
            var text = string.Empty;
            text += "⛹🏻 <b>Players</b> ⛹🏻\n";

            foreach(var p in players)
                text += GetFormattedPlayer(p) + "\n";

            return text;
        }

        public string GetFormattedPlayer(Player player) {
            var text = string.Empty;
            text += "\nPlayer Id: " + player.Id;

            text += "\nFirst name: " + player.FirstName;
            text += "\nLast name: " + player.LastName;
            text += "\nPosition: " + player.Position;

            if(player.HeightFeet != default)
                text += "\nHeight Feet: " + player.HeightFeet;
            if(player.HeightInches != default)
                text += "\nHeight Inches: " + player.HeightInches;

            if(player.Team is not null) {
                text += GetFormattedTeam(player.Team, "  ");
            }

            return text;
        }

        public string GetFormattedTeam(Team team, string tab = "") {
            var text = string.Empty;

            text += tab + "\nTeam Id: " + team.Id;
            text += tab + "\nAbbreviation: " + team.Abbreviation;
            text += tab + "\nFull Name: " + team.FullName;
            text += tab + "\nName: " + team.Name; 
            text += tab + "\nCity: " + team.City;
            text += tab + "\nConference: " + team.Conference;
            text += tab + "\nDivision: " + team.Division;

            return text;
        }

        public string GetFormattedGames(List<src.Models.Game> games) {
            var text = string.Empty;
            text += "🏀 <b>Games</b> 🏀\n";

            foreach(var g in games)
                text += GetFormattedGame(g) + "\n";

            return text;
        }

        public string GetFormattedGame(src.Models.Game game) {
            var text = string.Empty;
            text +=  "\nGame Id: " + game.Id;
            text += "\nDate: " + game.Date.ToShortDateString();
            if(game.HomeTeam is not null)
                text += "\nHome Team:" + GetFormattedTeam(game.HomeTeam);
            text += "\nHome Team Score: " + game.HomeTeamScore;
            text += "\nPeriod: " + game.Period;
            text += "\nPost season: " + game.Postseason.ToString();
            text += "\nSeason: " + game.Season;
            text += "\nStatus: " + game.Status;
            if(!string.IsNullOrEmpty(game.Time)) {
                text += "\nTime: " + game.Time;
            }
            if(game.VisitorTeam is not null)
                text += "\nVisitor Team:" + GetFormattedTeam(game.VisitorTeam);
            text += "\nVisitor Team Score: " + game.VisitorTeamScore;

            return text;
        }

        public string GetFormattedStats(List<Stats> stats) {
            var text = string.Empty;
            text += "📈 <b>Stats</b> 📈\n";

            foreach(var s in stats)
                text += GetFormattedStat(s) + "\n";

            return text;
        }

        public string GetFormattedStat(Stats stats) {
            var text = string.Empty;
            text +=  "\nId: " + stats.id;
            text +=  "\nast: " + stats.ast;
            text +=  "\nblk: " + stats.blk;
            text +=  "\ndreb: " + stats.dreb;
            text +=  "\nfg3_pct: " + stats.fg3_pct ?? default!;
            text +=  "\nfg3a: " + stats.fg3a;
            text +=  "\nfg3m: " + stats.fg3m;
            text +=  "\nfg_pct: " + stats.fg_pct;
            text +=  "\nfga: " + stats.fga;
            text +=  "\nfgm: " + stats.fgm;
            text +=  "\nft_pct: " + stats.ft_pct ?? default!;
            text +=  "\nfta: " + stats.fta;
            text +=  "\nftm: " + stats.ftm;

            if(stats.game is not null)
                text +=  GetFormattedGame(stats.game);

            text +=  "\nmin: " + stats.min;
            text +=  "\noreb: " + stats.oreb;
            text +=  "\npf: " + stats.pf;
           
            if(stats.player is not null)
                text +=  GetFormattedPlayer(stats.player);

            text +=  "\npts: " + stats.pts;
            text +=  "\nreb: " + stats.reb;
            text +=  "\nstl: " + stats.stl;
            
            if(stats.team is not null)
                text +=  GetFormattedTeam(stats.team);
            
            text +=  "\nturnover: " + stats.turnover;

            return text;
        }

        public string GetFormattedVideos(List<FavoriteVideo> videos) {
            var text = string.Empty;
            text += "🎥 <b>Saved videos</b> 🎥\n";

            foreach(var v in videos)
                text += GetFormattedVideo(v) + "\n";

            return text;
        }

        private string GetFormattedVideo(FavoriteVideo video) {
            var text = "\nTitle: " + video.VideoTitle;
            text += "\nUrl: " + video.VideoUrl;
            return text;
        }
    }
}