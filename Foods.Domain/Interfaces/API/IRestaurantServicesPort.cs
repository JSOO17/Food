using Foods.Domain.Models;

namespace Foods.Domain.Interfaces.API
{
    public interface IRestaurantServicesPort
    {
        Task<RestaurantModel> CreateRestaurant(RestaurantModel restaurant);
        Task<RestaurantEmployeesModel> CreateEmployeeRestaurant(RestaurantEmployeesModel restaurant, long userId);
        Task<List<ItemRestaurantModel>> GetRestaurants(int page, int count);
    }
}
