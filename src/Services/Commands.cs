using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace src.Services
{
    public static class Commands
    {
        public static string MainMenuMessage  = @"Welcome to the NBA Bot! Please choose an action:
⛹🏻All players - view all players
🔵Specific Team - view specific team
🏀All games - view all games 
📈Stats - view statistics 
🎥Add Favourite Video - add your favourite video to Database 
📆Reminder - set reminder about game
⛹🏻‍♂️Specific Player - view specific player by ID
🎥View videos - view your saved videos ";

        public static string AllPlayers = "⛹🏻All players";
        public static string SpecificTeam = "🔵Specific Team";
        public static string AllGames = "🏀All games";
        public static string Stats = "📈Stats";
        public static string AddFavVideo = "🎥Add Favourite Video";
        public static string Reminder = "📆Reminder";
        public static string SpecificPlayer = "⛹🏻‍♂️Specific Player";
        public static string ViewVideos = "🎥View videos";
        public static ReplyKeyboardMarkup GetMainMenuKeyboard() {
            var firstListButtons = new List<KeyboardButton>();
            var secondListButtons = new List<KeyboardButton>();

            firstListButtons.Add(new KeyboardButton(AllPlayers));
            firstListButtons.Add(new KeyboardButton(SpecificTeam));
            firstListButtons.Add(new KeyboardButton(AllGames));
            firstListButtons.Add(new KeyboardButton(AddFavVideo));

            secondListButtons.Add(new KeyboardButton(Stats));
            secondListButtons.Add(new KeyboardButton(ViewVideos));
            secondListButtons.Add(new KeyboardButton(Reminder));
            secondListButtons.Add(new KeyboardButton(SpecificPlayer));

            var keyboard = new ReplyKeyboardMarkup(
                new List<List<KeyboardButton>>() { firstListButtons, secondListButtons }) { ResizeKeyboard = true };

            return keyboard;
        }
        public static bool IsMainMenuCommand(string text) {
            bool isMainMenuCommand = false;

            if(text == AllPlayers) return true;
            if(text == SpecificTeam) return true;
            if(text == AllGames) return true;
            if(text == AddFavVideo) return true;
            if(text == Stats) return true;
            if(text == ViewVideos) return true;
            if(text == Reminder) return true;
            if(text == SpecificPlayer) return true;
            
            return isMainMenuCommand;
        }
    }
}