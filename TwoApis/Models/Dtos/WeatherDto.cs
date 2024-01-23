using System.Text.Json.Serialization;

namespace TwoApis.Models.Dtos
{
    public class ForecastDto
    {
        [JsonPropertyName("day")]
        public string Day { get; set; }

        [JsonPropertyName("temperature")]
        public string Temperature { get; set; }

        [JsonPropertyName("wind")]
        public string Wind { get; set; }
    }

    public class WeatherDto
    {
        [JsonPropertyName("temperature")]
        public string Temperature { get; set; }

        [JsonPropertyName("wind")]
        public string Wind { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("forecast")]
        public List<ForecastDto> Forecast { get; set; }
    }
}
