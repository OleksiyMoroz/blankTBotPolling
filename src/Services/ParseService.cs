using System.Text.Json;
using System.Text.Json.Serialization;
using src.Models;

namespace src.Services
{
    public class ParseService
    {
        public List<Player> DataToPlayers(string raw) {
            var parsed = JsonSerializer.Deserialize<DataPlayers>(raw);
            return parsed?.Players ?? default!;
        }

        public Player DataToPlayer(string raw) {
            var parsed = JsonSerializer.Deserialize<Player>(raw);
            return parsed ?? default!;
        }

        public Team DataToTeam(string raw) {
            var parsed = JsonSerializer.Deserialize<Team>(raw);
            return parsed ?? default!;
        }

        public List<Game> DataToGames(string raw) {
            var parsed = JsonSerializer.Deserialize<DataGames>(raw);
            return parsed?.Games ?? default!;
        }

        public List<Stats> DataToStats(string raw) {
            var parsed = JsonSerializer.Deserialize<DataStats>(raw);
            return parsed?.Stats ?? default!;
        }
    }

    public class DataPlayers {
        [JsonPropertyName("data")]
        public List<Player> Players { get; set; }
    }

    public class DataGames {
        [JsonPropertyName("data")]
        public List<Game> Games { get; set; }
    }

    public class DataStats {
        [JsonPropertyName("data")]
        public List<Stats> Stats { get; set; }
    }
}