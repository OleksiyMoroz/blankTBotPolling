using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace src.Models
{
    public class Team
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("abbreviation")]
        public string? Abbreviation { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("conference")]
        public string? Conference { get; set; }

        [JsonProperty("division")]
        public string? Division { get; set; }

        [JsonProperty("full_name")]
        public string? FullName { get; set; }
        
        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}