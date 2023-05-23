using Foods.Domain.Models;

namespace Foods.Domain.Interfaces.SPI
{
    public interface IRestaurantPersistencePort
    {
        Task<RestaurantModel> CreateRestaurant(RestaurantModel restaurant);
        Task<RestaurantEmployeesModel> CreateEmployeeRestaurant(RestaurantEmployeesModel restaurant);
        Task<bool> IsOwnerByRestaurant(long RestaurantId, long userId);
        Task<List<ItemRestaurantModel>> GetRestaurants(int page, int count);
    }
}
