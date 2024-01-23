using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwoApis.Models.Dtos;
using TwoApis.Models.ViewModels;
using TwoApis.Services;

namespace TwoApiTests
{
    [TestClass]
    public class WeatherServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task GetWeatherForCityAsync_Throws_Exeption_If_not_Connected()
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                });
            HttpClient mockClient = new HttpClient(mockHandler.Object);
            WeatherService weatherService = new WeatherService(mockClient);

            //Act
            await weatherService.GetWeatherForCityAsync("test-city");
        }

        [TestMethod]
        public async Task GetWeatherForCityAsync_Returns_Correct_Result()
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>() )
                .ReturnsAsync(new HttpResponseMessage 
                {  
                    StatusCode = HttpStatusCode.OK, 
                    Content = new StringContent("{\r\n    \"temperature\": \"test-temp\",\r\n    \"wind\": \"test-wind\",\r\n    \"description\": \"test-description\",\r\n    \"city\": \"test-city\"\r\n}")
                });
            HttpClient mockClient = new HttpClient (mockHandler.Object);
            WeatherService wWeatherService = new WeatherService( mockClient);

            //--
            //IpInfoService ipInfoService = new IpInfoService(mockClient);
            //string city = await ipInfoService.GetCityAsync("1.1.1.1");

            //Act
            var result = await wWeatherService.GetWeatherForCityAsync("test-city");
            WeatherViewModel weatherViewModel = new WeatherViewModel()
            {
                Temperature = result.Temperature,
                Wind = result.Wind,
                Description = result.Description,
                //Correct way to solve this?
                City = "test-city"
            };

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("test-temp", weatherViewModel.Temperature);
            Assert.AreEqual("test-wind", weatherViewModel.Wind);
            Assert.AreEqual("test-description", weatherViewModel.Description);
            Assert.AreEqual("test-city", weatherViewModel.City);
        }

    }
}
