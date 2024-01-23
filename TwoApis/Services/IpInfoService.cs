using System.Text.Json;
using TwoApis.Models.Dtos;

namespace TwoApis.Services
{
    public interface IIpInfoService
    {
        Task<string> GetCityAsync(string ip);
    }
    public class IpInfoService : IIpInfoService
    {
        private HttpClient _client;
        public IpInfoService(HttpClient client)
        {
            _client = client;
        }
        public IpInfoService() : this (new HttpClient())  { }

        public async Task<string> GetCityAsync(string ip)
        {
            var result = await _client.GetAsync($"http://ip-api.com/json/{ip}");
            result.EnsureSuccessStatusCode();

            IpInfoDto ipInfo = JsonSerializer.Deserialize<IpInfoDto>(await result.Content.ReadAsStringAsync());
            return ipInfo.City;
        }
    }
}
