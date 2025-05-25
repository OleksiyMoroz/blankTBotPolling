using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace src.Models
{
    public class Game
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("home_team")]
        public Team HomeTeam { get; set; }

        [JsonProperty("home_team_score")]
        public double HomeTeamScore { get; set; }

        [JsonProperty("period")]
        public double Period { get; set; }

        [JsonProperty("postseason")]
        public bool Postseason { get; set; }

        [JsonProperty("season")]
        public int Season { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("time")]
        public string? Time { get; set; }

        [JsonProperty("visitor_team")]
        public Team VisitorTeam { get; set; }

        [JsonProperty("visitor_team_score")]
        public double VisitorTeamScore { get; set; }
    }
}