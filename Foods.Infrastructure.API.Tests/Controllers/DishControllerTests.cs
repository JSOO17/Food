using Foods.Application.DTO.Request;
using Foods.Application.DTO.Response;
using Foods.Application.Services.Interfaces;
using Foods.Domain.Exceptions;
using Foods.Domain.HttpClients.Interfaces;
using Foods.Infrastructure.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

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
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

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

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new DishController(mockDishServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

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
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
               .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
               .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));


            mockDishServices
                .Setup(p => p.CreateDish(It.IsAny<DishRequestDTO>()))
                .Throws(new ValidationModelException("name is required"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new DishController(mockDishServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

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
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

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

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new DishController(mockDishServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

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
                .Returns(Task.FromResult(new List<DishResponseDTO>
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
                        }));

            var controller = new DishController(mockDishServices.Object, mockLogger.Object, mockserMicroHttpClient.Object);

            var response = (OkObjectResult)await controller.GetDishes(1, 1, 1);

            Assert.AreEqual(200, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(List<DishResponseDTO>));

            var dishes = response.Value as List<DishResponseDTO>;

            Assert.IsNotNull(dishes);
            Assert.AreEqual(1, dishes.Count);

            Assert.AreEqual("test dish", dishes[0].Name);
            Assert.AreEqual("to testing", dishes[0].Description);
            Assert.AreEqual(1, dishes[0].CategoryId);
            Assert.IsTrue(dishes[0].IsActive);
            Assert.AreEqual(1, dishes[0].Price);
            Assert.AreEqual(1, dishes[0].RestaurantId);
            Assert.AreEqual("whatever.com/test.png", dishes[0].UrlImagen);
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
