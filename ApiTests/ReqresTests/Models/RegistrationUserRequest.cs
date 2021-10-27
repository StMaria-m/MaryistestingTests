using Newtonsoft.Json;

namespace ApiTests.ReqresTests.Models
{
    public class RegistrationUserRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
