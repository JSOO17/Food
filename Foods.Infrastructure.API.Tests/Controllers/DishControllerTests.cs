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
    public class DishControllerTests
    {
        [TestMethod]
        public async Task CreateDishSuccessfull()
        {
            var mockDishServices = new Mock<IDishServices>();
            var mockLogger = new Mock<ILogger<DishController>>();
            var mockserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockDishServices
                .Setup(p => p.CreateDish(It.IsAny<DishRequestDTO>()))
                .Returns(Task.FromResult(new DishResponseDTO
                {
                    Id = 1,
                    Name = "test dish",
                    Description = "to testing",
                    CategoryId = 1,
                    IsActive = true,
                    Price = 1,
                    RestaurantId = 1,
                    UrlImagen = "whatever.com/test.png"
                }));

            var controller = new DishController(mockDishServices.Object, mockLogger.Object, mockserMicroHttpClient.Object);

            var dishRequest = new DishRequestDTO
            {
                Name = "test dish",
                Description = "to testing",
                CategoryId = 1,
                IsActive = true,
                Price = 1,
                RestaurantId = 1,
                UrlImagen = "whatever.com/test.png"
            };

            var response = (OkObjectResult)await controller.CreateDish(dishRequest);

            Assert.AreEqual(200, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(DishResponseDTO));

            var dishResponse = response.Value as DishResponseDTO;

            Assert.IsNotNull(dishResponse);
            Assert.AreEqual("test dish", dishResponse.Name);
            Assert.AreEqual("to testing", dishResponse.Description);
            Assert.AreEqual(1, dishResponse.CategoryId);
            Assert.IsTrue(dishResponse.IsActive);
            Assert.AreEqual(1, dishResponse.Price);
            Assert.AreEqual(1, dishResponse.RestaurantId);
            Assert.AreEqual("whatever.com/test.png", dishResponse.UrlImagen);
        }

        [TestMethod]
        public async Task CreateDishError()
        {
            var mockDishServices = new Mock<IDishServices>();
            var mockLogger = new Mock<ILogger<DishController>>();
            var mockserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockDishServices
                .Setup(p => p.CreateDish(It.IsAny<DishRequestDTO>()))
                .Throws(new Exception());

            var controller = new DishController(mockDishServices.Object, mockLogger.Object, mockserMicroHttpClient.Object);

            var dishRequest = new DishRequestDTO
            {
                Name = "test dish",
                Description = "to testing",
                CategoryId = 1,
                IsActive = true,
                Price = 1,
                RestaurantId = 1,
                UrlImagen = "whatever.com/test.png"
            };

            var response = (ObjectResult)await controller.CreateDish(dishRequest);

            Assert.AreEqual(500, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(500, apiResult.StatusCode);
            Assert.AreEqual("Something was wrong", apiResult.Message);
        }

        [TestMethod]
        public async Task CreateDishBadRequest()
        {
            var mockDishServices = new Mock<IDishServices>();
            var mockLogger = new Mock<ILogger<DishController>>();
            var mockserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockDishServices
                .Setup(p => p.CreateDish(It.IsAny<DishRequestDTO>()))
                .Throws(new ValidationModelException("name is required"));

            var controller = new DishController(mockDishServices.Object, mockLogger.Object, mockserMicroHttpClient.Object);

            var dishRequest = new DishRequestDTO
            {
                Name = "test dish",
                Description = "to testing",
                CategoryId = 1,
                IsActive = true,
                Price = 1,
                RestaurantId = 1,
                UrlImagen = "whatever.com/test.png"
            };

            var response = (ObjectResult)await controller.CreateDish(dishRequest);

            Assert.AreEqual(400, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(400, apiResult.StatusCode);
            Assert.AreEqual("Errors: name is required", apiResult.Message);
        }

        [TestMethod]
        public async Task UpdateDishSuccessfull()
        {
            var mockDishServices = new Mock<IDishServices>();
            var mockLogger = new Mock<ILogger<DishController>>();
            var mockserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockDishServices
                .Setup(p => p.CreateDish(It.IsAny<DishRequestDTO>()))
                .Returns(Task.FromResult(new DishResponseDTO
                {
                    Id = 2,
                    Name = "test dish",
                    Description = "to testing",
                    CategoryId = 1,
                    IsActive = true,
                    Price = 1,
                    RestaurantId = 1,
                    UrlImagen = "whatever.com/test.png"
                }));

            var controller = new DishController(mockDishServices.Object, mockLogger.Object, mockserMicroHttpClient.Object);

            var dishRequest = new DishRequestDTO
            {
                Name = "test dish",
                Description = "to testing",
                CategoryId = 1,
                IsActive = true,
                Price = 1,
                RestaurantId = 1,
                UrlImagen = "whatever.com/test.png"
            };

            var response = (OkObjectResult)await controller.UpdateDish(2, dishRequest);

            Assert.AreEqual(200, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(200, apiResult.StatusCode);
            Assert.AreEqual("Dish updated successfull", apiResult.Message);
        }

        public async Task UpdateDishError()
        {
            var mockDishServices = new Mock<IDishServices>();
            var mockLogger = new Mock<ILogger<DishController>>();
            var mockserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockDishServices
                .Setup(p => p.UpdateDish(It.IsAny<long>(), It.IsAny<DishRequestDTO>(), It.IsAny<long>()))
                .Throws(new Exception());

            var controller = new DishController(mockDishServices.Object, mockLogger.Object, mockserMicroHttpClient.Object);

            var dishRequest = new DishRequestDTO
            {
                Name = "test dish",
                Description = "to testing",
                CategoryId = 1,
                IsActive = true,
                Price = 1,
                RestaurantId = 1,
                UrlImagen = "whatever.com/test.png"
            };

            var response = (ObjectResult)await controller.UpdateDish(1, dishRequest);

            Assert.AreEqual(500, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(500, apiResult.StatusCode);
            Assert.AreEqual("Something was wrong", apiResult.Message);
        }

        [TestMethod]
        public async Task GetDishesSuccessfull()
        {
            var mockDishServices = new Mock<IDishServices>();
            var mockLogger = new Mock<ILogger<DishController>>();
            var mockserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockDishServices
                .Setup(p => p.GetDishes(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>()))
                .Returns(Task.FromResult(new List<CategoryDishesResponseDTO>
                {
                    new CategoryDishesResponseDTO
                    {
                        NameCategory = "xdf",
                        Dishes = new List<DishResponseDTO>
                        {
                            new DishResponseDTO
                            {
                                Id = 1,
                                Name = "test dish",
                                Description = "to testing",
                                CategoryId = 1,
                                IsActive = true,
                                Price = 1,
                                RestaurantId = 1,
                                UrlImagen = "whatever.com/test.png"
                            }
                        }
                    }
                }));

            var controller = new DishController(mockDishServices.Object, mockLogger.Object, mockserMicroHttpClient.Object);

            var response = (OkObjectResult)await controller.GetDishes(1, 1, 1);

            Assert.AreEqual(200, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(List<CategoryDishesResponseDTO>));

            var dishes = response.Value as List<CategoryDishesResponseDTO>;

            Assert.IsNotNull(dishes);
            Assert.AreEqual(1, dishes.Count);
            Assert.AreEqual(1, dishes[0].Dishes.Count);
            Assert.AreEqual("xdf", dishes[0].NameCategory);
            Assert.AreEqual("test dish", dishes[0].Dishes[0].Name);
            Assert.AreEqual("to testing", dishes[0].Dishes[0].Description);
            Assert.AreEqual(1, dishes[0].Dishes[0].CategoryId);
            Assert.IsTrue(dishes[0].Dishes[0].IsActive);
            Assert.AreEqual(1, dishes[0].Dishes[0].Price);
            Assert.AreEqual(1, dishes[0].Dishes[0].RestaurantId);
            Assert.AreEqual("whatever.com/test.png", dishes[0].Dishes[0].UrlImagen);
        }

        [TestMethod]
        public async Task GetDishesError()
        {
            var mockDishServices = new Mock<IDishServices>();
            var mockLogger = new Mock<ILogger<DishController>>();
            var mockserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockDishServices
                .Setup(p => p.GetDishes(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>()))
                .Throws(new Exception());

            var controller = new DishController(mockDishServices.Object, mockLogger.Object, mockserMicroHttpClient.Object);

            var response = (ObjectResult)await controller.GetDishes(1, 1, 1);

            Assert.AreEqual(500, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(500, apiResult.StatusCode);
            Assert.AreEqual("Something was wrong", apiResult.Message);
        }
    }
}
