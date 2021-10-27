using Newtonsoft.Json;

namespace ApiTests.ReqresTests.Models
{
    class UpdateUserDataRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("job")]
        public string Job { get; set; }
    }
}
