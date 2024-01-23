using System.Text.Json;
using TwoApis.Models.Dtos;

namespace TwoApis.Services
{
    public interface IWeatherService
    {
        Task<WeatherDto> GetWeatherForCityAsync(string city);
    }
    public class WeatherService : IWeatherService
    {
        private HttpClient _client;
        public WeatherService(HttpClient client)
        {
            _client = client;
        }
        public WeatherService() : this(new HttpClient()) { }

        public async Task<WeatherDto> GetWeatherForCityAsync(string city)
        {
            var result = await _client.GetAsync($"https://goweather.herokuapp.com/weather/{city}");
            result.EnsureSuccessStatusCode();

            WeatherDto weather = JsonSerializer.Deserialize<WeatherDto>(await result.Content.ReadAsStringAsync());
            return weather;
        }
    }
}
