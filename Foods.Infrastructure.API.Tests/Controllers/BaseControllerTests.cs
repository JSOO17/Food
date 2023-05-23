using Foods.Domain.Exceptions;
using Foods.Domain.HttpClients.Interfaces;
using Foods.Infrastructure.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Foods.Infrastructure.API.Tests.Controllers
{
    [TestClass]
    public class BaseControllerTests
    {
        [TestMethod]
        public async Task ValidateTokenSuccessfull()
        {
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = null }));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "insaneTokenBro";

            var controller = new BaseController(mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            await controller.ValidateToken();
        }

        [TestMethod]
        public async Task ValidateTokenIsNotValid()
        {
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            mockUserMicroHttpClient
                .Setup(p => p.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError, Content = null }));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "insaneTokenBro";

            var controller = new BaseController(mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            await Assert.ThrowsExceptionAsync<TokenIsNotValidException>(async () => await controller.ValidateToken());
        }

        [TestMethod]
        public void IsOwnerYes()
        {
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            var controller = new BaseController(mockUserMicroHttpClient.Object);

            controller.IsOwner(2);
        }

        [TestMethod]
        public void IsOwnerNo()
        {
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            var controller = new BaseController(mockUserMicroHttpClient.Object);

            Assert.ThrowsException<RoleHasNotPermissionException>(() =>
            {
                controller.IsOwner(1);
            });
        }

        [TestMethod]
        public void GetPayloadSuccesfull()
        {
            var mockUserMicroHttpClient = new Mock<IUserMicroHttpClient>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkdXJhbnBAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ5dW5lbmZpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiZXhwIjoxNjg0NDQxMTM5fQ.MGSLslRnszvP03furuac2LkNTh6wnTGLBo0xmy5RhGo";

            var controller = new BaseController(mockUserMicroHttpClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var payload = controller.GetPayload();

            Assert.AreEqual(1, payload.RoleId);
            Assert.AreEqual(3, payload.UserId);
        }
    }
}
