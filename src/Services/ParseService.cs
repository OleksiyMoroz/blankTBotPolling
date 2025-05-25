using Newtonsoft.Json;
using src.Models;

namespace src.Services
{
    public class ParseService
    {
        public List<Player> DataToPlayers(string raw) {
            var parsed = JsonConvert.DeserializeObject<DataPlayers>(raw);
            return parsed?.Players ?? default!;
        }

        public Player DataToPlayer(string raw) {
            var parsed = JsonConvert.DeserializeObject<Player>(raw);
            return parsed ?? default!;
        }

        public Team DataToTeam(string raw) {
            var parsed = JsonConvert.DeserializeObject<Team>(raw);
            return parsed ?? default!;
        }

        public List<Game> DataToGames(string raw) {
            var parsed = JsonConvert.DeserializeObject<DataGames>(raw);
            return parsed?.Games ?? default!;
        }

        public List<Stats> DataToStats(string raw) {
            var parsed = JsonConvert.DeserializeObject<DataStats>(raw);
            return parsed?.Stats ?? default!;
        }
    }

    public class DataPlayers {
        [JsonProperty("data")]
        public List<Player> Players { get; set; }
    }

    public class DataGames {
        [JsonProperty("data")]
        public List<Game> Games { get; set; }
    }

    public class DataStats {
        [JsonProperty("data")]
        public List<Stats> Stats { get; set; }
    }
}