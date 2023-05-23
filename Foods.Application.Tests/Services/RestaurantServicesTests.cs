using Foods.Application.DTO.Request;
using Foods.Application.Services;
using Foods.Domain.Interfaces.API;
using Foods.Domain.Models;
using Moq;

namespace Foods.Application.Tests.Services
{
    [TestClass]
    public class RestaurantServicesTests
    {
        [TestMethod]
        public async Task CreateRestaurantSuccessfull()
        {
            var mockServicesPort = new Mock<IRestaurantServicesPort>();

            mockServicesPort
                .Setup(p => p.CreateRestaurant(It.IsAny<RestaurantModel>()))
                .Returns(Task.FromResult(new RestaurantModel
                {
                    Id = 1,
                    Name = "test Restaurant",
                    Address = "test 112",
                    Cellphone = "+134134",
                    Nit = 3141234,
                    OwnerId = 1,
                    UrlLogo = "whatever.com/test.png"
                }));

            var services = new RestaurantServices(mockServicesPort.Object);

            var request = new RestaurantRequestDTO
            {
                Name = "test Restaurant",
                Address = "test 112",
                Cellphone = "+134134",
                Nit = 3141234,
                OwnerId = 1,
                UrlLogo = "whatever.com/test.png"
            };

            var response = await services.CreateRestaurant(request);

            Assert.IsNotNull(response);
            Assert.AreEqual("test Restaurant", response.Name);
            Assert.AreEqual("test 112", response.Address);
            Assert.AreEqual("+134134", response.Cellphone);
            Assert.AreEqual(3141234, response.Nit);
            Assert.AreEqual(1, response.OwnerId);
            Assert.AreEqual("whatever.com/test.png", response.UrlLogo);
        }

        [TestMethod]
        public async Task GetRestaurantsSuccessfull()
        {
            var mockServicesPort = new Mock<IRestaurantServicesPort>();

            mockServicesPort
                .Setup(p => p.GetRestaurants(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new List<ItemRestaurantModel>
                {
                    new ItemRestaurantModel
                    {
                        Id = 1,
                        Name = "xdf",
                        UrlLogo = "test.png"
                    }
                }));

            var services = new RestaurantServices(mockServicesPort.Object);

            var restaurants = await services.GetRestaurants(1, 1);

            Assert.IsNotNull(restaurants);
            Assert.AreEqual(1, restaurants.Count);
            Assert.AreEqual("xdf", restaurants[0].Name);
            Assert.AreEqual("test.png", restaurants[0].UrlLogo);
        }
    }
}
