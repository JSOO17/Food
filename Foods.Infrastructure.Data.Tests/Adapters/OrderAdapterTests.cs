using Foods.Domain.Models;
using Foods.Domain.Utils;
using Foods.Infrastructure.Data.Adapters;
using Foods.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Foods.Infrastructure.Data.Tests.Adapters
{
    [TestClass]
    public class OrderAdapterTests
    {
        private foodContext _context;
        
        private void Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<foodContext>()
                                                .UseInMemoryDatabase("food")
                                                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            _context = new foodContext(optionsBuilder.Options);
        }

        private async Task CleanUp()
        {
            await _context.Database.EnsureDeletedAsync();
        }

        private async Task SeedDatabase()
        {
            await _context.Restaurants.AddRangeAsync(new List<Restaurant>()
            {
                new Restaurant
                {
                    Name = "test Restaurant",
                    Address = "test 112",
                    Cellphone = "+134134",
                    Nit = 3141234,
                    OwnerId = 1,
                    UrlLogo = "whatever.com/test.png"
                },
                new Restaurant
                {
                    Name = "test Restaurant2",
                    Address = "test 1122",
                    Cellphone = "+1341342",
                    Nit = 31412342,
                    OwnerId = 2,
                    UrlLogo = "whatever.com/test2.png"
                },
                new Restaurant
                {
                    Name = "test Restaurant3",
                    Address = "test 1123",
                    Cellphone = "+1341343",
                    Nit = 31412343,
                    OwnerId = 3,
                    UrlLogo = "whatever.com/test3.png"
                }
            });

            await _context.Orders.AddRangeAsync(new List<Order>()
            {
                new Order
                {
                    ChefId = 1,
                    ClientId = 1,
                    State = OrderStates.Pending,
                    Date = DateTime.UtcNow,
                    RestaurantId = 1
                },
                new Order
                {
                    ChefId = 1,
                    ClientId = 3,
                    State = OrderStates.InPreparation,
                    Date = DateTime.UtcNow,
                    RestaurantId = 1
                },
                new Order
                {
                    ChefId = 1,
                    ClientId = 2,
                    State = OrderStates.Ready,
                    Date = DateTime.UtcNow,
                    RestaurantId = 1
                },
                new Order
                {
                    ChefId = 1,
                    ClientId = 2,
                    State = OrderStates.Pending,
                    Date = DateTime.UtcNow,
                    RestaurantId = 1
                },
                new Order
                {
                    ChefId = 1,
                    ClientId = 2,
                    State = OrderStates.Pending,
                    Date = DateTime.UtcNow,
                    RestaurantId = 1
                }
            });

            await _context.Dishes.AddRangeAsync(new List<Dish>()
            {
                new Dish
                {
                    Name = "test dish",
                    Description = "to testing",
                    CategoryId = 1,
                    IsActive = true,
                    Price = 1,
                    RestaurantId = 1,
                    UrlImagen = "whatever.com/test.png"
                },
                new Dish
                {
                    Name = "test dish2",
                    Description = "to testing2",
                    CategoryId = 2,
                    IsActive = true,
                    Price = 2,
                    RestaurantId = 1,
                    UrlImagen = "whatever2.com/test.png"
                },
                new Dish
                {
                    Name = "test dish3",
                    Description = "to testing3",
                    CategoryId = 1,
                    IsActive = true,
                    Price = 3,
                    RestaurantId = 1,
                    UrlImagen = "whatever3.com/test.png"
                },
                new Dish
                {
                    Name = "test dish4",
                    Description = "to testing4",
                    CategoryId = 2,
                    IsActive = true,
                    Price = 1,
                    RestaurantId = 1,
                    UrlImagen = "whatever4.com/test.png"
                },
                new Dish
                {
                    Name = "test dish5",
                    Description = "to testing5",
                    CategoryId = 3,
                    IsActive = true,
                    Price = 1,
                    RestaurantId = 1,
                    UrlImagen = "whatever5.com/test.png"
                }
            });

            await _context.Orderdishes.AddRangeAsync(new List<Orderdish>()
            {
                new Orderdish
                {
                    DishId = 1,
                    Cuantity = 1,
                    OrderId = 1
                }
            });

            await _context.SaveChangesAsync();
        }

        [TestMethod]
        public async Task CreateOrderSuccessFull()
        {
            Setup();
            await SeedDatabase();

            var adapter = new OrderAdapter(_context);

            var order = await adapter.CreateOrder(new OrderModel
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

            Assert.IsNotNull(order);
            Assert.AreEqual(45, order.ClientId);
            Assert.AreEqual(DateTime.Parse("2023-04-01"), order.Date);
            Assert.AreEqual(1, order.ChefId);
            Assert.AreEqual(1, order.RestaurantId);
            Assert.AreEqual(OrderStates.Pending, order.State);
            Assert.AreEqual(1, order.Dishes.Count);
            Assert.AreEqual(4, order.Dishes[0].Cuantity);
            Assert.AreEqual(3, order.Dishes[0].DishId);
            Assert.AreEqual(6, order.Dishes[0].OrderId);

            await CleanUp();
        }

        [TestMethod]
        public async Task HasClientOrdersTrue()
        {
            Setup();
            await SeedDatabase();

            var adapter = new OrderAdapter(_context);

            var hasClientOrder = await adapter.HasClientOrders(1);

            Assert.IsTrue(hasClientOrder);

            await CleanUp();
        }

        [TestMethod]
        public async Task HasClientOrdersFalse()
        {
            Setup();
            await SeedDatabase();

            var adapter = new OrderAdapter(_context);

            var hasClientOrder = await adapter.HasClientOrders(6);

            Assert.IsFalse(hasClientOrder);

            await CleanUp();
        }

        [TestMethod]
        public async Task GetOrdersSuccessfullPageOne()
        {
            Setup();
            await SeedDatabase();

            var adapter = new OrderAdapter(_context);

            var orders = await adapter.GetOrders(new OrderFiltersModel
            {
                State = OrderStates.Pending
            }, 1, 2, 1);

            Assert.AreEqual(2, orders.Count);

            await CleanUp();
        }

        [TestMethod]
        public async Task GetOrdersSuccessfullPageTwo()
        {
            Setup();
            await SeedDatabase();

            var adapter = new OrderAdapter(_context);

            var orders = await adapter.GetOrders(new OrderFiltersModel
            {
                State = OrderStates.Pending
            }, 2, 2, 1);

            Assert.AreEqual(1, orders.Count);

            await CleanUp();
        }
    }
}
