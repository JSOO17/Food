using FluentValidation;
using Foods.Domain.Exceptions;
using Foods.Domain.Interfaces.SPI;
using Foods.Domain.Models;
using Foods.Domain.UserCases;
using Moq;

namespace Foods.Domain.Tests.Usecases
{
    [TestClass]
    public class DishUsercasesTests
    {
        [TestMethod]
        public async Task CreateDishSuccessFull()
        {
            var servicesPersistencePort = new Mock<IDishPersistencePort>();

            var useCases = new DishUsercases(servicesPersistencePort.Object);

            servicesPersistencePort
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

            var dishModel = new DishModel
            {
                Id = 1,
                Name = "test dish",
                Description = "to testing",
                CategoryId = 1,
                IsActive = true,
                Price = 1,
                RestaurantId = 1,
                UrlImagen = "whatever.com/test.png"
            };

            var dish = await useCases.CreateDish(dishModel);

            Assert.IsNotNull(dish);
            Assert.AreEqual("test dish", dish.Name);
            Assert.AreEqual("to testing", dish.Description);
            Assert.AreEqual(1, dish.CategoryId);
            Assert.IsTrue(dish.IsActive);
            Assert.AreEqual(1, dish.Price);
            Assert.AreEqual(1, dish.RestaurantId);
            Assert.AreEqual("whatever.com/test.png", dish.UrlImagen);
        }

        [TestMethod]
        public async Task CreateDishIsNotValid()
        {
            var servicesPersistencePort = new Mock<IDishPersistencePort>();

            var useCases = new DishUsercases(servicesPersistencePort.Object);

            servicesPersistencePort
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

            var dishModel = new DishModel
            {
                Id = 1,
                Name = "test dish",
                CategoryId = 1,
                IsActive = true,
                Price = 1,
                RestaurantId = 1,
                UrlImagen = "whatever.com/test.png"
            };

            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(async () =>
            {
                await useCases.CreateDish(dishModel);
            });

            Assert.IsNotNull(exception);
            Assert.AreEqual("Validation failed: \r\n -- Description: The description can not be empty. Severity: Error\r\n -- Description: The description can not be null. Severity: Error", exception.Message);
        }

        [TestMethod]
        public async Task UpdateDishSuccessFull()
        {
            var servicesPersistencePort = new Mock<IDishPersistencePort>();

            var useCases = new DishUsercases(servicesPersistencePort.Object);

            servicesPersistencePort
                .Setup(p => p.UpdateDish(It.IsAny<long>(), It.IsAny<DishModel>()))
                .Returns(Task.CompletedTask);

            servicesPersistencePort
                .Setup(p => p.GetRestaurantByDishId(It.IsAny<long>()))
                .Returns(Task.FromResult(new RestaurantModel
                {
                    OwnerId = 1
                }));

            var dishModel = new DishModel
            {
                Name = "test dish",
                Description = "to testing",
                CategoryId = 1,
                IsActive = true,
                Price = 1,
                RestaurantId = 1,
                UrlImagen = "whatever.com/test.png"
            };

            await useCases.UpdateDish(1, dishModel, 1);
        }

        [TestMethod]
        public async Task UpdateDishRoleHasNotPermission()
        {
            var servicesPersistencePort = new Mock<IDishPersistencePort>();

            var useCases = new DishUsercases(servicesPersistencePort.Object);

            servicesPersistencePort
                .Setup(p => p.UpdateDish(It.IsAny<long>(), It.IsAny<DishModel>()))
                .Returns(Task.CompletedTask);

            servicesPersistencePort
                .Setup(p => p.GetRestaurantByDishId(It.IsAny<long>()))
                .Returns(Task.FromResult(new RestaurantModel
                {
                    OwnerId = 1
                }));

            var dishModel = new DishModel
            {
                Name = "test dish",
                Description = "to testing",
                CategoryId = 1,
                IsActive = true,
                Price = 1,
                RestaurantId = 1,
                UrlImagen = "whatever.com/test.png"
            };

            var exception = await Assert.ThrowsExceptionAsync<RoleHasNotPermissionException>(async () =>
            {
                await useCases.UpdateDish(1, dishModel, 2);
            });

            Assert.IsNotNull(exception);
            Assert.AreEqual("You are not the owner of the restaurant", exception.Message);

        }

        [TestMethod]
        public async Task GetDishesSuccessFull()
        {
            var servicesPersistencePort = new Mock<IDishPersistencePort>();

            var useCases = new DishUsercases(servicesPersistencePort.Object);

            servicesPersistencePort
                .Setup(p => p.GetDishes(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>()))
                .Returns(Task.FromResult(new List<CategoryDishesModel>
                {
                    new CategoryDishesModel
                    {
                        NameCategory = "xdf",
                        Dishes = new List<DishModel>
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
                        }
                    }
                }));

            var dishes = await useCases.GetDishes(1, 1, 1);

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
    }
}
