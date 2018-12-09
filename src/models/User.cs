using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace models
{
    [JsonObject ("user")]
    public class User
    {
        [JsonProperty (PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty (PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty (PropertyName = "description")]
        public string Description { get; set; }
    }
}