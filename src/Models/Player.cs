using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace src.Models
{
    public class Player
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        [JsonPropertyName("height_feet")]
        public double? HeightFeet { get; set; }

        [JsonPropertyName("height_inches")]
        public double? HeightInches { get; set; }

        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        [JsonPropertyName("position")]
        public string? Position { get; set; }
        [JsonPropertyName("weight_pounds")]
        public double? WeightPounds { get; set; }

        [JsonPropertyName("team")]
        public Team? Team { get; set; }
    }
}