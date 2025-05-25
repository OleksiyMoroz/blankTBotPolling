using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace src.Models
{
    public class Game
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("home_team")]
        public Team HomeTeam { get; set; }

        [JsonPropertyName("home_team_score")]
        public double HomeTeamScore { get; set; }

        [JsonPropertyName("period")]
        public double Period { get; set; }

        [JsonPropertyName("postseason")]
        public bool Postseason { get; set; }

        [JsonPropertyName("season")]
        public int Season { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("time")]
        public string? Time { get; set; }

        [JsonPropertyName("visitor_team")]
        public Team VisitorTeam { get; set; }

        [JsonPropertyName("visitor_team_score")]
        public double VisitorTeamScore { get; set; }
    }
}