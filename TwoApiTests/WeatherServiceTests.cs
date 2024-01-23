using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwoApis.Models.Dtos;
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

            //Act
            var result = await wWeatherService.GetWeatherForCityAsync("test-city");

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("test-temp", result.Temperature);
            Assert.AreEqual("test-wind", result.Wind);
            Assert.AreEqual("test-description", result.Description);
            Assert.AreEqual("test-city", result.City);
        }

    }
}
