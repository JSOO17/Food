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
    public class RestaurantControllerTests
    {
        [TestMethod]
        public async Task CreateRestaurantSuccessfull()
        {
            var mockRestaurantServices = new Mock<IRestaurantServices>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();
            var mockLogger = new Mock<ILogger<RestaurantController>>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

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

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.MGSLslRnszvP03furuac2LkNTh6wnTGLBo0xmy5RhGo";

            var controller = new RestaurantController(mockRestaurantServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

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
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();
            var mockLogger = new Mock<ILogger<RestaurantController>>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

            mockRestaurantServices
                .Setup(p => p.CreateRestaurant(It.IsAny<RestaurantRequestDTO>()))
                .Throws(new ValidationModelException("name is required"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.MGSLslRnszvP03furuac2LkNTh6wnTGLBo0xmy5RhGo";

            var controller = new RestaurantController(mockRestaurantServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

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
        public async Task CreateRestaurantUserIsNotAdmin()
        {
            var mockRestaurantServices = new Mock<IRestaurantServices>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();
            var mockLogger = new Mock<ILogger<RestaurantController>>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

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

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new RestaurantController(mockRestaurantServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

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

            Assert.AreEqual(403, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(403, apiResult.StatusCode);
            Assert.AreEqual("You dont have permission", apiResult.Message);
        }

        [TestMethod]
        public async Task CreateRestaurantTokenIsNotValid()
        {
            var mockRestaurantServices = new Mock<IRestaurantServices>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();
            var mockLogger = new Mock<ILogger<RestaurantController>>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = null }));

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

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "f.f.f";

            var controller = new RestaurantController(mockRestaurantServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

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

            Assert.AreEqual(401, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(401, apiResult.StatusCode);
            Assert.AreEqual("Errors: The token is not valid", apiResult.Message);
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

        [TestMethod]
        public async Task CreateEmployeeSuccessfull()
        {
            var mockRestaurantServices = new Mock<IRestaurantServices>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();
            var mockLogger = new Mock<ILogger<RestaurantController>>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

            mockRestaurantServices
                .Setup(p => p.CreateEmployeeRestaurant(It.IsAny<RestaurantEmployeesRequestDTO>(), It.IsAny<long>()))
                .Returns(Task.FromResult(new RestaurantEmployeesResponseDTO
                {
                    EmployeeId = 1,
                    RestaurantId = 2
                }));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.MGSLslRnszvP03furuac2LkNTh6wnTGLBo0xmy5RhGo";

            var controller = new RestaurantController(mockRestaurantServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var request = new RestaurantEmployeesRequestDTO
            {
                EmployeeId = 1,
                RestaurantId = 2
            };

            var response = (OkObjectResult)await controller.CreateEmployee(request);

            Assert.AreEqual(200, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(RestaurantEmployeesResponseDTO));

            var employee = response.Value as RestaurantEmployeesResponseDTO;

            Assert.IsNotNull(employee);
            Assert.AreEqual(1, employee.EmployeeId);
            Assert.AreEqual(2, employee.RestaurantId);
        }

        [TestMethod]
        public async Task CreateEmployeeBadRequest()
        {
            var mockRestaurantServices = new Mock<IRestaurantServices>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();
            var mockLogger = new Mock<ILogger<RestaurantController>>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

            mockRestaurantServices
                .Setup(p => p.CreateEmployeeRestaurant(It.IsAny<RestaurantEmployeesRequestDTO>(), It.IsAny<long>()))
                .Throws(new ValidationModelException("employeeId is required"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.MGSLslRnszvP03furuac2LkNTh6wnTGLBo0xmy5RhGo";

            var controller = new RestaurantController(mockRestaurantServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var request = new RestaurantEmployeesRequestDTO
            {
                EmployeeId = 1,
                RestaurantId = 2
            };

            var response = (ObjectResult)await controller.CreateEmployee(request);

            Assert.AreEqual(400, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(400, apiResult.StatusCode);
            Assert.AreEqual("Errors: employeeId is required", apiResult.Message);
        }

        [TestMethod]
        public async Task CreateEmployeeTokenIsNotValid()
        {
            var mockRestaurantServices = new Mock<IRestaurantServices>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();
            var mockLogger = new Mock<ILogger<RestaurantController>>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = null }));

            mockRestaurantServices
                .Setup(p => p.CreateEmployeeRestaurant(It.IsAny<RestaurantEmployeesRequestDTO>(), It.IsAny<long>()))
                .Returns(Task.FromResult(new RestaurantEmployeesResponseDTO
                {
                    EmployeeId = 1,
                    RestaurantId = 2
                }));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "f.f.f";

            var controller = new RestaurantController(mockRestaurantServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var request = new RestaurantEmployeesRequestDTO
            {
                EmployeeId = 1,
                RestaurantId = 2
            };

            var response = (ObjectResult)await controller.CreateEmployee(request);

            Assert.AreEqual(401, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(401, apiResult.StatusCode);
            Assert.AreEqual("Errors: The token is not valid", apiResult.Message);
        }

        [TestMethod]
        public async Task CreateEmployeeUserIsNotOwner()
        {
            var mockRestaurantServices = new Mock<IRestaurantServices>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();
            var mockLogger = new Mock<ILogger<RestaurantController>>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

            mockRestaurantServices
                .Setup(p => p.CreateEmployeeRestaurant(It.IsAny<RestaurantEmployeesRequestDTO>(), It.IsAny<long>()))
                .Throws(new RoleHasNotPermissionException("You do not have permission"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.MGSLslRnszvP03furuac2LkNTh6wnTGLBo0xmy5RhGo";

            var controller = new RestaurantController(mockRestaurantServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var request = new RestaurantEmployeesRequestDTO
            {
                EmployeeId = 1,
                RestaurantId = 2
            };

            var response = (ObjectResult)await controller.CreateEmployee(request);

            Assert.AreEqual(403, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(403, apiResult.StatusCode);
            Assert.AreEqual("Errors: You do not have permission", apiResult.Message);
        }
    }

}
