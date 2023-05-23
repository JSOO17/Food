using Foods.Domain.Models;
using Foods.Infrastructure.Data.Adapters;
using Foods.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Foods.Infrastructure.Data.Tests.Adapters
{
    [TestClass]
    public class DishAdapterTests
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
            await _context.Categories.AddRangeAsync(new List<Category>()
            {
                new Category
                {
                    Name = "categoryTest1",
                    Description = "test"
                },
                new Category
                {
                    Name = "categoryTest2",
                    Description = "test"
                },
                new Category
                {
                    Name = "categoryTest3",
                    Description = "test"
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

            await _context.SaveChangesAsync();
        }

        [TestMethod]
        public async Task CreateDishSuccesfull()
        {
            Setup();
            await SeedDatabase();

            var adapter = new DishAdapter(_context);

            var dishModel = new DishModel
            {
                Name = "test dish3",
                Description = "to testing3",
                CategoryId = 3,
                IsActive = false,
                Price = 3,
                RestaurantId = 3,
                UrlImagen = "whatever.com/test3.png"
            };

            var dish = await adapter.CreateDish(dishModel);

            Assert.IsNotNull(dish);
            Assert.AreEqual(3, dish.Id);
            Assert.AreEqual("test dish3", dish.Name);
            Assert.AreEqual("to testing3", dish.Description);
            Assert.AreEqual(3, dish.CategoryId);
            Assert.IsFalse(dish.IsActive);
            Assert.AreEqual(3, dish.Price);
            Assert.AreEqual(3, dish.RestaurantId);
            Assert.AreEqual("whatever.com/test3.png", dish.UrlImagen);

            await CleanUp();
        }

        [TestMethod]
        public async Task UpdateDishSuccesfull()
        {
            Setup();
            await SeedDatabase();

            var adapter = new DishAdapter(_context);

            var dishModel = new DishModel
            {
                Name = "test dish updated",
                Description = "to testing updated",
                CategoryId = 4,
                IsActive = false,
                Price = 4,
                RestaurantId = 4,
                UrlImagen = "whatever.com/testupdated.png"
            };

            await adapter.UpdateDish(2, dishModel);

            var dish = await _context.Dishes.Where(d => d.Id == 2).FirstOrDefaultAsync();

            Assert.IsNotNull(dish);
            Assert.AreEqual(2, dish.Id);
            Assert.AreEqual("test dish1", dish.Name);
            Assert.AreEqual("to testing updated", dish.Description);
            Assert.AreEqual(1, dish.CategoryId);
            Assert.IsFalse(dish.IsActive);
            Assert.AreEqual(4, dish.Price);
            Assert.AreEqual(1, dish.RestaurantId);
            Assert.AreEqual("whatever.com/test.png", dish.UrlImagen);

            await CleanUp();
        }

        [TestMethod]
        public async Task GetDishesSuccesfullPageOne()
        {
            Setup();
            await SeedDatabase();

            var adapter = new DishAdapter(_context);

            var dishes = await adapter.GetDishes(1, 2, 1);

            Assert.AreEqual(2, dishes.Count);
            Assert.IsNotNull(dishes[0]);
            Assert.AreEqual("categoryTest1", dishes[0].NameCategory);
            Assert.AreEqual(2, dishes[0].Dishes.Count);

            Assert.AreEqual("test dish", dishes[0].Dishes[0].Name);
            Assert.AreEqual("to testing", dishes[0].Dishes[0].Description);
            Assert.AreEqual(1, dishes[0].Dishes[0].CategoryId);
            Assert.IsTrue(dishes[0].Dishes[0].IsActive);
            Assert.AreEqual(1, dishes[0].Dishes[0].Price);
            Assert.AreEqual(1, dishes[0].Dishes[0].RestaurantId);
            Assert.AreEqual("whatever.com/test.png", dishes[0].Dishes[0].UrlImagen);

            Assert.AreEqual("test dish3", dishes[0].Dishes[1].Name);
            Assert.AreEqual("to testing3", dishes[0].Dishes[1].Description);
            Assert.AreEqual(1, dishes[0].Dishes[1].CategoryId);
            Assert.IsTrue(dishes[0].Dishes[1].IsActive);
            Assert.AreEqual(3, dishes[0].Dishes[1].Price);
            Assert.AreEqual(1, dishes[0].Dishes[1].RestaurantId);
            Assert.AreEqual("whatever3.com/test.png", dishes[0].Dishes[1].UrlImagen);

            await CleanUp();
        }

        [TestMethod]
        public async Task GetDishesSuccesfullPageTwo()
        {
            Setup();
            await SeedDatabase();

            var adapter = new DishAdapter(_context);

            var dishes = await adapter.GetDishes(2, 2, 1);

            Assert.AreEqual(1, dishes.Count);
            Assert.IsNotNull(dishes[0]);
            Assert.AreEqual("categoryTest3", dishes[0].NameCategory);
            Assert.AreEqual(1, dishes[0].Dishes.Count);

            Assert.AreEqual("test dish5", dishes[0].Dishes[0].Name);
            Assert.AreEqual("to testing5", dishes[0].Dishes[0].Description);
            Assert.AreEqual(3, dishes[0].Dishes[0].CategoryId);
            Assert.IsTrue(dishes[0].Dishes[0].IsActive);
            Assert.AreEqual(1, dishes[0].Dishes[0].Price);
            Assert.AreEqual(1, dishes[0].Dishes[0].RestaurantId);
            Assert.AreEqual("whatever5.com/test.png", dishes[0].Dishes[0].UrlImagen);

            await CleanUp();
        }
    }
}
