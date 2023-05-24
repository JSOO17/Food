using Foods.Application.DTO.Request;
using Foods.Application.Services;
using Foods.Domain.Interfaces.API;
using Foods.Domain.Models;
using Foods.Domain.Utils;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Foods.Application.Tests.Services
{
    [TestClass]
    public class OrderServicesTests
    {
        [TestMethod]
        public async Task CreateOrderSuccessfull()
        {
            var servicesPort = new Mock<IOrderServicesPort>();

            servicesPort
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

            var services = new OrderServices(servicesPort.Object);

            var order = await services.CreateOrder(new OrderRequestDTO());

            Assert.IsNotNull(order);
            Assert.AreEqual(45, order.ClientId);
            Assert.AreEqual(DateTime.Parse("2023-04-01"), order.Date);
            Assert.AreEqual(1, order.ChefId);
            Assert.AreEqual(1, order.RestaurantId);
            Assert.AreEqual(OrderStates.Pending, order.State);
            Assert.AreEqual(1, order.Dishes.Count);
            Assert.AreEqual(4, order.Dishes[0].Cuantity);
            Assert.AreEqual(3, order.Dishes[0].DishId);
            Assert.AreEqual(4, order.Dishes[0].OrderId);
        }
    }
}
