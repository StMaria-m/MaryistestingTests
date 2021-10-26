using Newtonsoft.Json;

namespace ApiTests.JokeApiTests.Models
{
    public class Flags
    {
        [JsonProperty("nsfw")]
        public bool Nsfw { get; set; }
        [JsonProperty("religious")]
        public bool Religious { get; set; }
        [JsonProperty("political")]
        public bool Political { get; set; }
        [JsonProperty("racist")]
        public bool Racist { get; set; }
        [JsonProperty("sexist")]
        public bool Sexist { get; set; }
        [JsonProperty("explicit")]
        public bool Explicit { get; set; }
    }
}
