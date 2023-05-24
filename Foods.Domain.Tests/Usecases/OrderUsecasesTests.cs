using FluentValidation;
using Foods.Domain.Exceptions;
using Foods.Domain.Interfaces.SPI;
using Foods.Domain.Models;
using Foods.Domain.UserCases;
using Foods.Domain.Utils;
using Moq;

namespace Foods.Domain.Tests.Usecases
{
    [TestClass]
    public class OrderUsecasesTests
    {
        [TestMethod]
        public async Task CreateOrderSuccessFull()
        {
            var servicesPersistencePort = new Mock<IOrderPersistencePort>();

            var useCases = new OrderUsecases(servicesPersistencePort.Object);

            servicesPersistencePort
                .Setup(p => p.HasClientOrders(It.IsAny<long>()))
                .Returns(Task.FromResult(false));

            servicesPersistencePort
                .Setup(p => p.CreateOrder(It.IsAny<OrderModel>()))
                .Returns(Task.FromResult(new OrderModel
                {
                    Id = 4,
                    ClientId = 45,
                    ChefId = 1,
                    Date = DateTime.Parse("2023-04-01"),
                    RestaurantId = 1,
                    State = OrderStates.Pending,
                    Dishes = new List<OrderDishModel>
                    {
                        new OrderDishModel
                        {
                            Cuantity = 4,
                            DishId = 3,
                            OrderId = 4
                        }
                    }
                }));

            var model = await useCases.CreateOrder(new OrderModel
            {
                ClientId = 45,
                ChefId = 1,
                Date = DateTime.Parse("2023-04-01"),
                RestaurantId = 1,
                State = OrderStates.Pending,
                Dishes = new List<OrderDishModel>
                    {
                        new OrderDishModel
                        {
                            Cuantity = 4,
                            DishId = 3
                        }
                    }
            });

            Assert.IsNotNull(model);
            Assert.AreEqual(45, model.ClientId);
            Assert.AreEqual(DateTime.Parse("2023-04-01"), model.Date);
            Assert.AreEqual(1, model.ChefId);
            Assert.AreEqual(1, model.RestaurantId);
            Assert.AreEqual(OrderStates.Pending, model.State);
            Assert.AreEqual(1, model.Dishes.Count);
            Assert.AreEqual(4, model.Dishes[0].Cuantity);
            Assert.AreEqual(3, model.Dishes[0].DishId);
            Assert.AreEqual(4, model.Dishes[0].OrderId);
        }

        [TestMethod]
        public async Task CreateOrderIsNotValid()
        {
            var servicesPersistencePort = new Mock<IOrderPersistencePort>();

            var useCases = new OrderUsecases(servicesPersistencePort.Object);

            servicesPersistencePort
                .Setup(p => p.HasClientOrders(It.IsAny<long>()))
                .Returns(Task.FromResult(false));

            servicesPersistencePort
                .Setup(p => p.CreateOrder(It.IsAny<OrderModel>()))
                .Returns(Task.FromResult(new OrderModel
                {
                    ClientId = 45,
                    Date = DateTime.Parse("2023-04-01"),
                    State = OrderStates.Pending,
                    Dishes = new List<OrderDishModel>
                    {
                        new OrderDishModel
                        {
                            Cuantity = 4,
                            DishId = 3
                        }
                    }
                }));

            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(async () =>
            {
                await useCases.CreateOrder(new OrderModel
                {
                    ClientId = 45,
                    ChefId = 1,
                    Date = DateTime.Parse("2023-04-01"),
                    State = OrderStates.Pending,
                    Dishes = new List<OrderDishModel>
                    {
                        new OrderDishModel
                        {
                            Cuantity = 4,
                            DishId = 3
                        }
                    }
                });
            });

            Assert.IsNotNull(exception);
            Assert.AreEqual("Validation failed: \r\n -- RestaurantId: The restaurantId can not be empty. Severity: Error", exception.Message);
        }

        [TestMethod]
        public async Task CreateOrderClientAlreadyHasOrder()
        {
            var servicesPersistencePort = new Mock<IOrderPersistencePort>();

            var useCases = new OrderUsecases(servicesPersistencePort.Object);

            servicesPersistencePort
                .Setup(p => p.HasClientOrders(It.IsAny<long>()))
                .Returns(Task.FromResult(true));

            servicesPersistencePort
                .Setup(p => p.CreateOrder(It.IsAny<OrderModel>()))
                .Returns(Task.FromResult(new OrderModel
                {
                    ClientId = 45,
                    Date = DateTime.Parse("2023-04-01"),
                    State = OrderStates.Pending,
                    Dishes = new List<OrderDishModel>
                    {
                        new OrderDishModel
                        {
                            Cuantity = 4,
                            DishId = 3
                        }
                    }
                }));

            var exception = await Assert.ThrowsExceptionAsync<ClientAlreadyHasOrderException>(async () =>
            {
                await useCases.CreateOrder(new OrderModel
                {
                    ClientId = 45,
                    ChefId = 1,
                    Date = DateTime.Parse("2023-04-01"),
                    RestaurantId = 1,
                    State = OrderStates.Pending,
                    Dishes = new List<OrderDishModel>
                    {
                        new OrderDishModel
                        {
                            Cuantity = 4,
                            DishId = 3
                        }
                    }
                });
            });

            Assert.IsNotNull(exception);
            Assert.AreEqual("You already have a order on pending, In Progress or Ready", exception.Message);
        }
    }
}
