using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTests.MyStoreApiTests.Models
{
    public class NewOrderRequest
    {
        [JsonProperty("customer")]
        public string Customer { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
