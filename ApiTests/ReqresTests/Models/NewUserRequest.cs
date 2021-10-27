using Newtonsoft.Json;

namespace ApiTests.ReqresTests.Models
{
    public class NewUserRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("job")]
        public string Job { get; set; }
    }
}
