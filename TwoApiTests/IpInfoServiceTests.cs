using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwoApis.Services;

namespace TwoApiTests
{
    [TestClass]
    public class IpInfoServiceTests
    {
        //testing correct return
        [TestMethod]
        public async Task GetCityAsync_ReturnsCorrectCity()
        {
            //Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"city\": \"test-city\"}")
                });
            HttpClient mockClient = new HttpClient(mockHandler.Object);
            IpInfoService service = new IpInfoService(mockClient);

            //Act
            string result = await service.GetCityAsync("1.1.1.1.1");

            //Assert
            Assert.AreEqual("test-city", result);
        }

        //Testing exception
        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task GetCityAsync_Throws_Exception_If_Conncetion_Not_Working()
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                { StatusCode = HttpStatusCode.BadGateway, });
            HttpClient mockClient = new HttpClient (mockHandler.Object);
            IpInfoService service = new IpInfoService(mockClient);

            //act
            await service.GetCityAsync("1.1.1.1");
        }
    }
}
