using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace src.Models
{
    public class Player
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("first_name")]
        public string? FirstName { get; set; }

        [JsonProperty("height_feet")]
        public double? HeightFeet { get; set; }

        [JsonProperty("height_inches")]
        public double? HeightInches { get; set; }

        [JsonProperty("last_name")]
        public string? LastName { get; set; }

        [JsonProperty("position")]
        public string? Position { get; set; }
        [JsonProperty("weight_pounds")]
        public double? WeightPounds { get; set; }

        [JsonProperty("team")]
        public Team? Team { get; set; }
    }
}