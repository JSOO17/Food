using Foods.Domain.Interfaces.SPI;
using Foods.Domain.Models;
using Foods.Infrastructure.Data.Mappers;
using Foods.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Foods.Infrastructure.Data.Adapters
{
    public class RestaurantAdapter : IRestaurantPersistencePort
    {
        private readonly foodContext _foodContext;

        public RestaurantAdapter(foodContext foodContext) 
        { 
            _foodContext = foodContext;
        }

        public async Task<RestaurantEmployeesModel> CreateEmployeeRestaurant(RestaurantEmployeesModel model)
        {
            await _foodContext.Restaurantemployees.AddAsync(new Restaurantemployee
            {
                RestaurantId = model.RestaurantId,
                EmployeeId = model.EmployeeId
            });

            await _foodContext.SaveChangesAsync();

            return model;
        }

        public async Task<RestaurantModel> CreateRestaurant(RestaurantModel restaurantModel)
        {
            var restaurant = RestaurantMapper.ToRestaurant(restaurantModel);

            await _foodContext.Restaurants.AddAsync(restaurant);

            await _foodContext.SaveChangesAsync();

            restaurantModel.Id = restaurant.Id;

            return restaurantModel;
        }

        public async Task<RestaurantModel?> GetRestaurantByEmployeeId(long employeeId)
        {
            var result = await (from employee in _foodContext.Restaurantemployees
                          join restaurant in _foodContext.Restaurants on employee.RestaurantId equals restaurant.Id
                          where employee.EmployeeId == employeeId
                          select restaurant).FirstOrDefaultAsync();

            return result == null ? null : RestaurantMapper.ToRestaurantModel(result);
        }

        public async Task<List<ItemRestaurantModel>> GetRestaurants(int page, int count)
        {
            var skip = page > 1 ? count * (page - 1) : 0;

            return await _foodContext.Restaurants
                        .Skip(skip)
                        .Take(count)
                        .Select(r => new ItemRestaurantModel
                        {
                            Name = r.Name,
                            UrlLogo = r.UrlLogo
                        })
                        .OrderByDescending(r => r.Name)
                        .ToListAsync();
        }

        public async Task<bool> IsOwnerByRestaurant(long restaurantId, long userId)
        {
            var restaurant = await _foodContext.Restaurants.Where(r => r.Id == restaurantId).FirstOrDefaultAsync();

            if (restaurant != null) 
            {
                return restaurant.OwnerId == userId;
            }

            return false;
        }
    }
}
