using JokeApiTests.Models;
using Newtonsoft.Json;

namespace JokeApiTests.Models
{
    public class NewJokeRequest 
    {
        [JsonProperty("formatVersion")]
        public int FormatVersion { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("joke")]
        public string Joke { get; set; }
        [JsonProperty("lang")]
        public string Lang { get; set; }
        [JsonProperty("flags")]
        public Flags Flags { get; set; }
    }    
}
