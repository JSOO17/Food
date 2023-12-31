﻿using FluentValidation;
using Foods.Application.DTO.Request;
using Foods.Application.DTO.Response;
using Foods.Application.Services.Interfaces;
using Foods.Domain.Exceptions;
using Foods.Domain.HttpClients.Interfaces;
using Foods.Domain.Utils;
using Foods.Infrastructure.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace Foods.Infrastructure.API.Tests.Controllers
{
    [TestClass]
    public class OrderControllerTests
    {
        [TestMethod]
        public async Task CreateOrderSuccessfull()
        {
            var mockServices = new Mock<IOrderServices>();
            var mockLogger = new Mock<ILogger<OrderController>>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

            mockServices
                .Setup(p => p.CreateOrder(It.IsAny<OrderRequestDTO>()))
                .Returns(Task.FromResult(new OrderResponseDTO
                {
                    ClientId = 45,
                    ChefId = 1,
                    Date = DateTime.Parse("2023-04-01"),
                    RestaurantId = 1,
                    State = OrderStates.Pending,
                    Dishes = new List<OrderDishResponseDTO>
                    {
                        new OrderDishResponseDTO
                        {
                            OrderId = 3,
                            Cuantity = 4,
                            DishId = 3
                        }
                    }
                }));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new OrderController(mockServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = (OkObjectResult)await controller.CreateOrder(new OrderRequestDTO
            {
                ClientId = 45,
                ChefId = 1,
                Date = DateTime.Parse("2023-04-01"),
                RestaurantId = 1,
                State = OrderStates.Pending,
                Dishes = new List<OrderDishRequestDTO>
                {
                    new OrderDishRequestDTO
                    {
                        Cuantity = 4,
                        DishId = 3
                    }
                }
            });

            Assert.AreEqual(200, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(OrderResponseDTO));

            var orderResponse = response.Value as OrderResponseDTO;

            Assert.IsNotNull(orderResponse);
            Assert.AreEqual(45, orderResponse.ClientId);
            Assert.AreEqual(DateTime.Parse("2023-04-01"), orderResponse.Date);
            Assert.AreEqual(1, orderResponse.ChefId);
            Assert.AreEqual(1, orderResponse.RestaurantId);
            Assert.AreEqual(OrderStates.Pending, orderResponse.State);
            Assert.AreEqual(1, orderResponse.Dishes.Count);
            Assert.AreEqual(4, orderResponse.Dishes[0].Cuantity);
            Assert.AreEqual(3, orderResponse.Dishes[0].DishId);
            Assert.AreEqual(3, orderResponse.Dishes[0].OrderId);
        }

        [TestMethod]
        public async Task CreateOrderTokenIsNotValid()
        {
            var mockServices = new Mock<IOrderServices>();
            var mockLogger = new Mock<ILogger<OrderController>>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = null }));

            mockServices
                .Setup(p => p.CreateOrder(It.IsAny<OrderRequestDTO>()))
                .Returns(Task.FromResult(new OrderResponseDTO
                {
                    ClientId = 45,
                    ChefId = 1,
                    Date = DateTime.Parse("2023-04-01"),
                    RestaurantId = 1,
                    State = OrderStates.Pending,
                    Dishes = new List<OrderDishResponseDTO>
                    {
                        new OrderDishResponseDTO
                        {
                            OrderId = 3,
                            Cuantity = 4,
                            DishId = 3
                        }
                    }
                }));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new OrderController(mockServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = (ObjectResult)await controller.CreateOrder(new OrderRequestDTO
            {
                ClientId = 45,
                ChefId = 1,
                Date = DateTime.Parse("2023-04-01"),
                RestaurantId = 1,
                State = OrderStates.Pending,
                Dishes = new List<OrderDishRequestDTO>
                {
                    new OrderDishRequestDTO
                    {
                        Cuantity = 4,
                        DishId = 3
                    }
                }
            });

            Assert.AreEqual(401, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(401, apiResult.StatusCode);
            Assert.AreEqual("Errors: The token is not valid", apiResult.Message);
        }

        [TestMethod]
        public async Task CreateOrderUserHasNotPermission()
        {
            var mockServices = new Mock<IOrderServices>();
            var mockLogger = new Mock<ILogger<OrderController>>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

            mockServices
                .Setup(p => p.CreateOrder(It.IsAny<OrderRequestDTO>()))
                .Throws(new RoleHasNotPermissionException("you dont have permission"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new OrderController(mockServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = (ObjectResult)await controller.CreateOrder(new OrderRequestDTO
            {
                ClientId = 45,
                ChefId = 1,
                Date = DateTime.Parse("2023-04-01"),
                RestaurantId = 1,
                State = OrderStates.Pending,
                Dishes = new List<OrderDishRequestDTO>
                {
                    new OrderDishRequestDTO
                    {
                        Cuantity = 4,
                        DishId = 3
                    }
                }
            });

            Assert.AreEqual(403, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(403, apiResult.StatusCode);
            Assert.AreEqual("Errors: you dont have permission", apiResult.Message);
        }

        [TestMethod]
        public async Task CreateOrderClientAlreadyHasOrder()
        {
            var mockServices = new Mock<IOrderServices>();
            var mockLogger = new Mock<ILogger<OrderController>>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

            mockServices
                .Setup(p => p.CreateOrder(It.IsAny<OrderRequestDTO>()))
                .Throws(new ClientAlreadyHasOrderException("you already have a order"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new OrderController(mockServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = (ObjectResult)await controller.CreateOrder(new OrderRequestDTO
            {
                ClientId = 45,
                ChefId = 1,
                Date = DateTime.Parse("2023-04-01"),
                RestaurantId = 1,
                State = OrderStates.Pending,
                Dishes = new List<OrderDishRequestDTO>
                {
                    new OrderDishRequestDTO
                    {
                        Cuantity = 4,
                        DishId = 3
                    }
                }
            });

            Assert.AreEqual(400, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(400, apiResult.StatusCode);
            Assert.AreEqual("Errors: you already have a order", apiResult.Message);
        }

        [TestMethod]
        public async Task CreateOrderClientValidationFailed()
        {
            var mockServices = new Mock<IOrderServices>();
            var mockLogger = new Mock<ILogger<OrderController>>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

            mockServices
                .Setup(p => p.CreateOrder(It.IsAny<OrderRequestDTO>()))
                .Throws(new ValidationException("the clientId is required"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new OrderController(mockServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = (ObjectResult)await controller.CreateOrder(new OrderRequestDTO
            {
                ClientId = 45,
                ChefId = 1,
                Date = DateTime.Parse("2023-04-01"),
                RestaurantId = 1,
                State = OrderStates.Pending,
                Dishes = new List<OrderDishRequestDTO>
                {
                    new OrderDishRequestDTO
                    {
                        Cuantity = 4,
                        DishId = 3
                    }
                }
            });

            Assert.AreEqual(400, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(400, apiResult.StatusCode);
            Assert.AreEqual("Errors: the clientId is required", apiResult.Message);
        }

        [TestMethod]
        public async Task CreateOrderError()
        {
            var mockServices = new Mock<IOrderServices>();
            var mockLogger = new Mock<ILogger<OrderController>>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

            mockServices
                .Setup(p => p.CreateOrder(It.IsAny<OrderRequestDTO>()))
                .Throws(new Exception());

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new OrderController(mockServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = (ObjectResult)await controller.CreateOrder(new OrderRequestDTO
            {
                ClientId = 45,
                ChefId = 1,
                Date = DateTime.Parse("2023-04-01"),
                RestaurantId = 1,
                State = OrderStates.Pending,
                Dishes = new List<OrderDishRequestDTO>
                {
                    new OrderDishRequestDTO
                    {
                        Cuantity = 4,
                        DishId = 3
                    }
                }
            });

            Assert.AreEqual(500, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(500, apiResult.StatusCode);
            Assert.AreEqual("Something was wrong", apiResult.Message);
        }

        [TestMethod]
        public async Task GetOrdersSuccesfull()
        {
            var mockServices = new Mock<IOrderServices>();
            var mockLogger = new Mock<ILogger<OrderController>>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

            mockServices
                .Setup(p => p.GetOrders(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>()))
                .Returns(Task.FromResult(new List<OrderResponseDTO>
                {
                    new OrderResponseDTO
                    {
                        Id = 4,
                        ClientId = 45,
                        ChefId = 1,
                        Date = DateTime.Parse("2023-04-01"),
                        RestaurantId = 1,
                        State = OrderStates.Pending,
                        Dishes = new List<OrderDishResponseDTO>
                        {
                            new OrderDishResponseDTO
                            {
                                Cuantity = 4,
                                DishId = 3,
                                OrderId = 4
                            }
                        }
                    }
                }));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new OrderController(mockServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = (OkObjectResult)await controller.GetOrders(OrderStates.Pending, 1, 1);

            Assert.AreEqual(200, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(List<OrderResponseDTO>));

            var orders = response.Value as List<OrderResponseDTO>;

            Assert.IsNotNull(orders);
            Assert.IsNotNull(orders[0]);
            Assert.AreEqual(1, orders.Count);
            Assert.AreEqual(45, orders[0].ClientId);
            Assert.AreEqual(DateTime.Parse("2023-04-01"), orders[0].Date);
            Assert.AreEqual(1, orders[0].ChefId);
            Assert.AreEqual(1, orders[0].RestaurantId);
            Assert.AreEqual(OrderStates.Pending, orders[0].State);
            Assert.AreEqual(1, orders[0].Dishes.Count);
            Assert.AreEqual(4, orders[0].Dishes[0].Cuantity);
            Assert.AreEqual(3, orders[0].Dishes[0].DishId);
            Assert.AreEqual(4, orders[0].Dishes[0].OrderId);
        }

        [TestMethod]
        public async Task GetOrdersTokenIsNotValid()
        {
            var mockServices = new Mock<IOrderServices>();
            var mockLogger = new Mock<ILogger<OrderController>>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = null }));

            mockServices
                .Setup(p => p.GetOrders(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>()))
                .Returns(Task.FromResult(new List<OrderResponseDTO>
                {
                    new OrderResponseDTO
                    {
                        Id = 4,
                        ClientId = 45,
                        ChefId = 1,
                        Date = DateTime.Parse("2023-04-01"),
                        RestaurantId = 1,
                        State = OrderStates.Pending,
                        Dishes = new List<OrderDishResponseDTO>
                        {
                            new OrderDishResponseDTO
                            {
                                Cuantity = 4,
                                DishId = 3,
                                OrderId = 4
                            }
                        }
                    }
                }));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new OrderController(mockServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = (ObjectResult)await controller.GetOrders(OrderStates.Pending, 1, 1);

            Assert.AreEqual(401, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(401, apiResult.StatusCode);
            Assert.AreEqual("Errors: The token is not valid", apiResult.Message);
        }

        [TestMethod]
        public async Task GetOrdersError()
        {
            var mockServices = new Mock<IOrderServices>();
            var mockLogger = new Mock<ILogger<OrderController>>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

            mockServices
                .Setup(p => p.GetOrders(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>()))
                .Throws(new Exception());

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new OrderController(mockServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = (ObjectResult)await controller.GetOrders(OrderStates.Pending, 1, 1);

            Assert.AreEqual(500, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(500, apiResult.StatusCode);
            Assert.AreEqual("Something was wrong", apiResult.Message);
        }

        [TestMethod]
        public async Task UpdateOrderSuccessfull()
        {
            var mockServices = new Mock<IOrderServices>();
            var mockLogger = new Mock<ILogger<OrderController>>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

            mockServices
                .Setup(p => p.UpdateOrder(It.IsAny<long>(), It.IsAny<OrderRequestDTO>(), It.IsAny<long>(),It.IsAny<string>()))
                .Returns(Task.FromResult(new OrderResponseDTO
                {
                    ClientId = 45,
                    ChefId = 1,
                    Date = DateTime.Parse("2023-04-01"),
                    RestaurantId = 1,
                    State = OrderStates.InPreparation,
                }));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new OrderController(mockServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = (OkObjectResult)await controller.UpdateOrder(1, new OrderRequestDTO
            {
                ClientId = 45,
                ChefId = 1,
                Date = DateTime.Parse("2023-04-01"),
                RestaurantId = 1,
                State = OrderStates.InPreparation
            });

            Assert.AreEqual(200, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(OrderResponseDTO));

            var orderResponse = response.Value as OrderResponseDTO;

            Assert.IsNotNull(orderResponse);
            Assert.AreEqual(45, orderResponse.ClientId);
            Assert.AreEqual(DateTime.Parse("2023-04-01"), orderResponse.Date);
            Assert.AreEqual(1, orderResponse.ChefId);
            Assert.AreEqual(1, orderResponse.RestaurantId);
            Assert.AreEqual(OrderStates.InPreparation, orderResponse.State);
        }

        [TestMethod]
        public async Task UpdateOrderTokenIsNotValid()
        {
            var mockServices = new Mock<IOrderServices>();
            var mockLogger = new Mock<ILogger<OrderController>>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = null }));

            mockServices
                .Setup(p => p.UpdateOrder(It.IsAny<long>(), It.IsAny<OrderRequestDTO>(), It.IsAny<long>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new OrderResponseDTO
                {
                    ClientId = 45,
                    ChefId = 1,
                    Date = DateTime.Parse("2023-04-01"),
                    RestaurantId = 1,
                    State = OrderStates.InPreparation,
                }));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new OrderController(mockServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = (ObjectResult)await controller.UpdateOrder(1, new OrderRequestDTO
            {
                ClientId = 45,
                ChefId = 1,
                Date = DateTime.Parse("2023-04-01"),
                RestaurantId = 1,
                State = OrderStates.Pending
            });

            Assert.AreEqual(401, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(401, apiResult.StatusCode);
            Assert.AreEqual("Errors: The token is not valid", apiResult.Message);
        }

        [TestMethod]
        public async Task UpdateOrderUserHasNotPermission()
        {
            var mockServices = new Mock<IOrderServices>();
            var mockLogger = new Mock<ILogger<OrderController>>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

            mockServices
                .Setup(p => p.UpdateOrder(It.IsAny<long>(), It.IsAny<OrderRequestDTO>(), It.IsAny<long>(), It.IsAny<string>()))
                .Throws(new RoleHasNotPermissionException("you dont have permission"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new OrderController(mockServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = (ObjectResult)await controller.UpdateOrder(1, new OrderRequestDTO
            {
                ClientId = 45,
                ChefId = 1,
                Date = DateTime.Parse("2023-04-01"),
                RestaurantId = 1,
                State = OrderStates.Pending,
                Dishes = new List<OrderDishRequestDTO>
                {
                    new OrderDishRequestDTO
                    {
                        Cuantity = 4,
                        DishId = 3
                    }
                }
            });

            Assert.AreEqual(403, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(403, apiResult.StatusCode);
            Assert.AreEqual("Errors: you dont have permission", apiResult.Message);
        }

        [TestMethod]
        public async Task UpdateOrderChangeState()
        {
            var mockServices = new Mock<IOrderServices>();
            var mockLogger = new Mock<ILogger<OrderController>>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

            mockServices
                .Setup(p => p.UpdateOrder(It.IsAny<long>(), It.IsAny<OrderRequestDTO>(), It.IsAny<long>(), It.IsAny<string>()))
                .Throws(new OrderStateChangeInvalidException("The order is not ready yet"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new OrderController(mockServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = (ObjectResult)await controller.UpdateOrder(1, new OrderRequestDTO
            {
                ClientId = 45,
                ChefId = 1,
                Date = DateTime.Parse("2023-04-01"),
                RestaurantId = 1,
                State = OrderStates.Pending,
            });

            Assert.AreEqual(400, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(400, apiResult.StatusCode);
            Assert.AreEqual("Errors: The order is not ready yet", apiResult.Message);
        }

        [TestMethod]
        public async Task UpdateOrderCodeIsInvalid()
        {
            var mockServices = new Mock<IOrderServices>();
            var mockLogger = new Mock<ILogger<OrderController>>();
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

            mockServices
                .Setup(p => p.UpdateOrder(It.IsAny<long>(), It.IsAny<OrderRequestDTO>(), It.IsAny<long>(), It.IsAny<string>()))
                .Throws(new OrderCodeInvalidException());

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.e2KrEkNGRn2eRp14RTWcx3B0JfxXsBxrdeuMQKup98c";

            var controller = new OrderController(mockServices.Object, mockLogger.Object, mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var response = (ObjectResult)await controller.UpdateOrder(1, new OrderRequestDTO
            {
                ClientId = 45,
                ChefId = 1,
                Date = DateTime.Parse("2023-04-01"),
                RestaurantId = 1,
                State = OrderStates.Delivered,
            });

            Assert.AreEqual(400, response.StatusCode);
            Assert.IsInstanceOfType(response.Value, typeof(ApiResult));

            var apiResult = response.Value as ApiResult;

            Assert.IsNotNull(apiResult);
            Assert.AreEqual(400, apiResult.StatusCode);
            Assert.AreEqual("Errors: The code is invalid", apiResult.Message);
        }
    }
}
