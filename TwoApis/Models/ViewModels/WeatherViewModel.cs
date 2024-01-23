using System.Text.Json.Serialization;

namespace TwoApis.Models.ViewModels
{
    public class WeatherViewModel
    {
        [JsonPropertyName("temperature")]
        public string Temperature { get; set; }

        [JsonPropertyName("wind")]
        public string Wind { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }
    }
}
