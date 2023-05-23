using Foods.Application.DTO.Request;
using Foods.Application.DTO.Response;
using Foods.Application.Services.Interfaces;
using Foods.Domain.Exceptions;
using Foods.Domain.HttpClients.Interfaces;
using Foods.Domain.Models;
using Foods.Infrastructure.API.Controllers;
using Foods.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Foods.Infrastructure.API.Tests.Controllers
{
    [TestClass]
    public class RestaurantControllerTests
    {
        [TestMethod]
        public async Task CreateRestaurantSuccessfull()
        {
            var mockRestaurantServices = new Mock<IRestaurantServices>();
            var mockserMicroHttpClient = new Mock<IUserMicroHttpClient>();
            var mockLogger = new Mock<ILogger<RestaurantController>>();

            mockRestaurantServices
                .Setup(p => p.CreateRestaurant(It.IsAny<RestaurantRequestDTO>()))
                .Returns(Task.FromResult(new RestaurantResponseDTO
                {
                    Id = 1,
                    Name = "test Restaurant",
                    Address = "test 112",
                    Cellphone = "+134134",
                    Nit = 3141234,
                    OwnerId = 1,
                    UrlLogo = "whatever.com/test.png"
                }));

            var controller = new RestaurantController(mockRestaurantServices.Object, mockLogger.Object, mockserMicroHttpClient.Object);

            var restaurantRequest = new RestaurantRequestDTO
            {
                Name = "test Restaurant",
                Address = "test 112",
                Cellphone = "+134134",
                Nit = 3141234,
                OwnerId = 1,
                UrlLogo = "whatever.com/test.png"
            };

            var response = (OkObjectResult)await controller.CreateRestaurant(restaurantRequest);

            Assert.AreEqual(200, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(RestaurantResponseDTO));

            var restaurantResponse = response.Value as RestaurantResponseDTO;

            Assert.IsNotNull(restaurantResponse);
            Assert.AreEqual("test Restaurant", restaurantResponse.Name);
            Assert.AreEqual("test 112", restaurantResponse.Address);
            Assert.AreEqual("+134134", restaurantResponse.Cellphone);
            Assert.AreEqual(3141234, restaurantResponse.Nit);
            Assert.AreEqual(1, restaurantResponse.OwnerId);
            Assert.AreEqual("whatever.com/test.png", restaurantResponse.UrlLogo);
        }

        [TestMethod]
        public async Task CreateRestaurantError()
        {
            var mockRestaurantServices = new Mock<IRestaurantServices>();
            var mockserMicroHttpClient = new Mock<IUserMicroHttpClient>();
            var mockLogger = new Mock<ILogger<RestaurantController>>();

            mockRestaurantServices
                .Setup(p => p.CreateRestaurant(It.IsAny<RestaurantRequestDTO>()))
                .Throws(new Exception());

            var controller = new RestaurantController(mockRestaurantServices.Object, mockLogger.Object, mockserMicroHttpClient.Object);

            var restaurantRequest = new RestaurantRequestDTO
            {
                Name = "test Restaurant",
                Address = "test 112",
                Cellphone = "+134134",
                Nit = 3141234,
                OwnerId = 1,
                UrlLogo = "whatever.com/test.png"
            };

            var response = (ObjectResult)await controller.CreateRestaurant(restaurantRequest);

            Assert.AreEqual(500, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(500, apiResult.StatusCode);
            Assert.AreEqual("Something was wrong", apiResult.Message);
        }

        [TestMethod]
        public async Task CreateRestaurantBadRequest()
        {
            var mockRestaurantServices = new Mock<IRestaurantServices>();
            var mockserMicroHttpClient = new Mock<IUserMicroHttpClient>();
            var mockLogger = new Mock<ILogger<RestaurantController>>();

            mockRestaurantServices
                .Setup(p => p.CreateRestaurant(It.IsAny<RestaurantRequestDTO>()))
                .Throws(new ValidationModelException("name is required"));

            var controller = new RestaurantController(mockRestaurantServices.Object, mockLogger.Object, mockserMicroHttpClient.Object);

            var restaurantRequest = new RestaurantRequestDTO
            {
                Name = "test Restaurant",
                Address = "test 112",
                Cellphone = "+134134",
                Nit = 3141234,
                OwnerId = 1,
                UrlLogo = "whatever.com/test.png"
            };

            var response = (ObjectResult)await controller.CreateRestaurant(restaurantRequest);

            Assert.AreEqual(400, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(400, apiResult.StatusCode);
            Assert.AreEqual("Errors: name is required", apiResult.Message);
        }

        [TestMethod]
        public async Task GetRestaurantsSuccessfull()
        {
            var mockRestaurantServices = new Mock<IRestaurantServices>();
            var mockserMicroHttpClient = new Mock<IUserMicroHttpClient>();
            var mockLogger = new Mock<ILogger<RestaurantController>>();

            mockRestaurantServices
                .Setup(p => p.GetRestaurants(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new List<ItemRestaurantResponseDTO>
                {
                    new ItemRestaurantResponseDTO
                    {
                        Id = 1,
                        Name = "xdf",
                        UrlLogo = "test.png"
                    }
                }));

            var controller = new RestaurantController(mockRestaurantServices.Object, mockLogger.Object, mockserMicroHttpClient.Object);

            var response = (OkObjectResult)await controller.GetRestaurants(1, 1);

            Assert.AreEqual(200, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(List<ItemRestaurantResponseDTO>));

            var restaurantsResponse = response.Value as List<ItemRestaurantResponseDTO>;

            Assert.IsNotNull(restaurantsResponse);
            Assert.AreEqual(1, restaurantsResponse.Count);
            Assert.AreEqual("xdf", restaurantsResponse[0].Name);
            Assert.AreEqual("test.png", restaurantsResponse[0].UrlLogo);
        }

        [TestMethod]
        public async Task GetRestaurantsError()
        {
            var mockRestaurantServices = new Mock<IRestaurantServices>();
            var mockserMicroHttpClient = new Mock<IUserMicroHttpClient>();
            var mockLogger = new Mock<ILogger<RestaurantController>>();

            mockRestaurantServices
                .Setup(p => p.GetRestaurants(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception());

            var controller = new RestaurantController(mockRestaurantServices.Object, mockLogger.Object, mockserMicroHttpClient.Object);

            var response = (ObjectResult)await controller.GetRestaurants(1, 1);

            Assert.AreEqual(500, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(500, apiResult.StatusCode);
            Assert.AreEqual("Something was wrong", apiResult.Message);
        }
    }
}
