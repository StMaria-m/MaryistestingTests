using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTests.MyStoreApiTests.Models
{
    public class NewProductRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")]
        public int Price { get; set; }
        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("tags")]
        public string Tags { get; set; }
    }
}
