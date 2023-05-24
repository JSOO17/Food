using Foods.Application.DTO.Request;
using Foods.Application.Services;
using Foods.Domain.Interfaces.API;
using Foods.Domain.Models;
using Moq;

namespace Foods.Application.Tests.Services
{
    [TestClass]
    public class DishServicesTests
    {
        [TestMethod]
        public async Task CreateDishSuccesfull()
        {
            var servicesPort = new Mock<IDishServicesPort>();

            servicesPort
                .Setup(p => p.CreateDish(It.IsAny<DishModel>()))
                .Returns(Task.FromResult(new DishModel
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

            var services = new DishServices(servicesPort.Object);

            var request = new DishRequestDTO
            {
                Name = "test dish",
                Description = "to testing",
                CategoryId = 1,
                IsActive = true,
                Price = 1,
                RestaurantId = 1,
                UrlImagen = "whatever.com/test.png"
            };

            var response = await services.CreateDish(request);

            Assert.IsNotNull(response);
            Assert.AreEqual("test dish", response.Name);
            Assert.AreEqual("to testing", response.Description);
            Assert.AreEqual(1, response.CategoryId);
            Assert.IsTrue(response.IsActive);
            Assert.AreEqual(1, response.Price);
            Assert.AreEqual(1, response.RestaurantId);
            Assert.AreEqual("whatever.com/test.png", response.UrlImagen);
        }

        [TestMethod]
        public async Task GetDishesSuccesfull()
        {
            var servicesPort = new Mock<IDishServicesPort>();

            servicesPort
                .Setup(p => p.GetDishes(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>()))
                .Returns(Task.FromResult(new List<DishModel>
                        {
                            new DishModel
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

            var services = new DishServices(servicesPort.Object);

            var dishes = await services.GetDishes(1, 1, 1);

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
    }
}
