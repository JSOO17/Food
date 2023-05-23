using Foods.Domain.Interfaces.SPI;
using Foods.Domain.Models;
using Foods.Infrastructure.Data.Mappers;
using Foods.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Foods.Infrastructure.Data.Adapters
{
    public class DishAdapter : IDishPersistencePort
    {
        private readonly foodContext _foodContext;

        public DishAdapter(foodContext foodContext)
        {
            _foodContext = foodContext;
        }
        public async Task<List<CategoryDishesModel>> GetDishes(int page, int count, long restaurantId)
        {
            var skip = page > 1 ? count * (page - 1) : 0;

            var categories = await (from dish in _foodContext.Dishes
                                    join category in _foodContext.Categories on dish.CategoryId equals category.Id
                                    where dish.RestaurantId == restaurantId
                                    select category)
                                    .Distinct()
                                    .Skip(skip)
                                    .Take(count)
                                    .ToListAsync();

            var dishes = new List<CategoryDishesModel>();

            categories.ForEach(c =>
                        {
                            var dish = _foodContext.Dishes.Where(d => d.RestaurantId == restaurantId && d.CategoryId == c.Id).ToList();
                            if (dish.Any())
                            {

                                dishes.Add(new CategoryDishesModel
                                {
                                    NameCategory = c.Name,
                                    Dishes = DishMapper.ToModel(dish)
                                });
                            }
                        });
            return dishes;
        }

        public async Task<DishModel> CreateDish(DishModel dishModel)
        {
            var dish = DishMapper.ToDish(dishModel);

            await _foodContext.Dishes.AddAsync(dish);

            await _foodContext.SaveChangesAsync();

            dishModel.Id = dish.Id;

            return dishModel;
        }

        public async Task<RestaurantModel?> GetRestaurantByDishId(long dishId)
        {
            var dish = await _foodContext.Dishes.Where(d => d.Id == dishId).FirstOrDefaultAsync();

            if (dish != null)
            {
                var restaurant = await _foodContext.Restaurants.Where(r => r.Id == dish.RestaurantId).FirstOrDefaultAsync();

                return restaurant != null ? RestaurantMapper.ToRestaurantModel(restaurant) : null;
            }

            return null;
        }

        public async Task UpdateDish(long id, DishModel dishModel)
        {
            var dish = await _foodContext.Dishes.Where(dish => dish.Id == id).FirstOrDefaultAsync();

            if (dish != null)
            {
                dish.Description = dishModel.Description;
                dish.Price = dishModel.Price;
                dish.IsActive = dishModel.IsActive;

                _foodContext.Entry(dish).State = EntityState.Modified;

                await _foodContext.SaveChangesAsync();
            }
        }
    }
}
