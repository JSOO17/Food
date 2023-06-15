using FluentValidation;
using Foods.Domain.Exceptions;
using Foods.Domain.Interfaces.SPI;
using Foods.Domain.Models;
using Foods.Domain.UserCases;
using Moq;

namespace Foods.Domain.Tests.Usecases
{
    [TestClass]
    public class RestaurantUsecasesTests
    {
        [TestMethod]
        public async Task CreateRestaurantSuccessfull()
        {
            var servicesPersistencePort = new Mock<IRestaurantPersistencePort>();

            var useCases = new RestaurantUsercases(servicesPersistencePort.Object);

            servicesPersistencePort
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

            var model = new RestaurantModel
            {
                Id = 1,
                Name = "test Restaurant",
                Address = "test 112",
                Cellphone = "+134134",
                Nit = 3141234,
                OwnerId = 1,
                UrlLogo = "whatever.com/test.png"
            };

            var restaurant = await useCases.CreateRestaurant(model);

            Assert.IsNotNull(restaurant);
            Assert.AreEqual("test Restaurant", restaurant.Name);
            Assert.AreEqual("test 112", restaurant.Address);
            Assert.AreEqual("+134134", restaurant.Cellphone);
            Assert.AreEqual(3141234, restaurant.Nit);
            Assert.AreEqual(1, restaurant.OwnerId);
            Assert.AreEqual("whatever.com/test.png", restaurant.UrlLogo);
        }

        [TestMethod]
        public async Task CreateRestaurantIsNotValid()
        {
            var servicesPersistencePort = new Mock<IRestaurantPersistencePort>();

            var useCases = new RestaurantUsercases(servicesPersistencePort.Object);

            servicesPersistencePort
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

            var model = new RestaurantModel
            {
                Id = 1,
                Address = "test 112",
                Cellphone = "+134134",
                Nit = 3141234,
                OwnerId = 1,
                UrlLogo = "whatever.com/test.png"
            };

            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(async () =>
            {
                await useCases.CreateRestaurant(model);
            });

            Assert.IsNotNull(exception);
            Assert.AreEqual("Validation failed: \r\n -- Name: The name can not be empty. Severity: Error\r\n -- Name: The name can not be null. Severity: Error", exception.Message);
        }

        [TestMethod]
        public async Task GetRestaurantsSuccessfull()
        {
            var servicesPersistencePort = new Mock<IRestaurantPersistencePort>();

            var useCases = new RestaurantUsercases(servicesPersistencePort.Object);

            servicesPersistencePort
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

            var restaurants = await useCases.GetRestaurants(1, 2);

            Assert.IsNotNull(restaurants);
            Assert.AreEqual(1, restaurants.Count);
            Assert.AreEqual("xdf", restaurants[0].Name);
            Assert.AreEqual("test.png", restaurants[0].UrlLogo);
        }

        [TestMethod]
        public async Task CreateEmployeeRestaurantSuccessfull()
        {
            var servicesPersistencePort = new Mock<IRestaurantPersistencePort>();

            var useCases = new RestaurantUsercases(servicesPersistencePort.Object);

            servicesPersistencePort
                .Setup(p => p.CreateEmployeeRestaurant(It.IsAny<RestaurantEmployeesModel>()))
                .Returns(Task.FromResult(new RestaurantEmployeesModel
                {
                    EmployeeId = 1,
                    RestaurantId = 3
                }));

            servicesPersistencePort
                .Setup(p => p.IsOwnerByRestaurant(It.IsAny<long>(), It.IsAny<long>()))
                .Returns(Task.FromResult(true));

            var model = new RestaurantEmployeesModel
            {
                EmployeeId = 1,
                RestaurantId = 3
            };

            var restaurantEmployee = await useCases.CreateEmployeeRestaurant(model, 1);

            Assert.IsNotNull(restaurantEmployee);
            Assert.AreEqual(3, restaurantEmployee.RestaurantId);
            Assert.AreEqual(1, restaurantEmployee.EmployeeId);
        }

        [TestMethod]
        public async Task CreateEmployeeRestaurantNotValid()
        {
            var servicesPersistencePort = new Mock<IRestaurantPersistencePort>();

            var useCases = new RestaurantUsercases(servicesPersistencePort.Object);

            servicesPersistencePort
                .Setup(p => p.CreateEmployeeRestaurant(It.IsAny<RestaurantEmployeesModel>()))
                .Returns(Task.FromResult(new RestaurantEmployeesModel
                {
                    RestaurantId = 3
                }));

            servicesPersistencePort
                .Setup(p => p.IsOwnerByRestaurant(It.IsAny<long>(), It.IsAny<long>()))
                .Returns(Task.FromResult(true));

            var model = new RestaurantEmployeesModel
            {
                RestaurantId = 3
            };

            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(async () =>
            {
                await useCases.CreateEmployeeRestaurant(model, 1);
            });

            Assert.IsNotNull(exception);
            Assert.AreEqual("Validation failed: \r\n -- EmployeeId: The employeeId can not be empty. Severity: Error", exception.Message);
        }

        [TestMethod]
        public async Task CreateEmployeeRestaurantUserIsNotOwner()
        {
            var servicesPersistencePort = new Mock<IRestaurantPersistencePort>();

            var useCases = new RestaurantUsercases(servicesPersistencePort.Object);

            servicesPersistencePort
                .Setup(p => p.CreateEmployeeRestaurant(It.IsAny<RestaurantEmployeesModel>()))
                .Returns(Task.FromResult(new RestaurantEmployeesModel
                {
                    RestaurantId = 3,
                    EmployeeId = 1
                }));

            servicesPersistencePort
                .Setup(p => p.IsOwnerByRestaurant(It.IsAny<long>(), It.IsAny<long>()))
                .Returns(Task.FromResult(false));

            var model = new RestaurantEmployeesModel
            {
                RestaurantId = 3,
                EmployeeId = 1
            };

            var exception = await Assert.ThrowsExceptionAsync<RoleHasNotPermissionException>(async () =>
            {
                await useCases.CreateEmployeeRestaurant(model, 1);
            });

            Assert.IsNotNull(exception);
            Assert.AreEqual("You are not the owner of the restaurant", exception.Message);
        }
    }
}
