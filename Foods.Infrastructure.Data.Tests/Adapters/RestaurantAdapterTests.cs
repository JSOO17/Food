using Foods.Domain.Models;
using Foods.Infrastructure.Data.Adapters;
using Foods.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Foods.Infrastructure.Data.Tests.Adapters
{
    [TestClass]
    public class RestaurantAdapterTests
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

            await _context.SaveChangesAsync();
        }

        [TestMethod]
        public async Task CreateRestaurantSuccesfull()
        {
            Setup();
            await SeedDatabase();

            var adapter = new RestaurantAdapter(_context);

            var model = new RestaurantModel
            {
                Name = "test Restaurant",
                Address = "test 112",
                Cellphone = "+134134",
                Nit = 3141234,
                OwnerId = 1,
                UrlLogo = "whatever.com/test.png"
            };

            var restaurant = await adapter.CreateRestaurant(model);

            Assert.IsNotNull(restaurant);
            Assert.AreEqual("test Restaurant", restaurant.Name);
            Assert.AreEqual("test 112", restaurant.Address);
            Assert.AreEqual("+134134", restaurant.Cellphone);
            Assert.AreEqual(3141234, restaurant.Nit);
            Assert.AreEqual(1, restaurant.OwnerId);
            Assert.AreEqual("whatever.com/test.png", restaurant.UrlLogo);

            await CleanUp();
        }

        [TestMethod]
        public async Task GetRestaurantsSuccesfullPageOne()
        {
            Setup();
            await SeedDatabase();

            var adapter = new RestaurantAdapter(_context);

            var restaurants = await adapter.GetRestaurants(1, 2);

            Assert.AreEqual(2, restaurants.Count());

            Assert.AreEqual("test Restaurant", restaurants[1].Name);
            Assert.AreEqual("whatever.com/test.png", restaurants[1].UrlLogo);

            Assert.AreEqual("test Restaurant2", restaurants[0].Name);
            Assert.AreEqual("whatever.com/test2.png", restaurants[0].UrlLogo);

            await CleanUp();
        }

        [TestMethod]
        public async Task GetRestaurantsSuccesfullPageTwo()
        {
            Setup();
            await SeedDatabase();

            var adapter = new RestaurantAdapter(_context);

            var restaurants = await adapter.GetRestaurants(2, 2);

            Assert.AreEqual(1, restaurants.Count());

            Assert.AreEqual("test Restaurant3", restaurants[0].Name);
            Assert.AreEqual("whatever.com/test3.png", restaurants[0].UrlLogo);
        }
    }
}
